using Api.Entities.Content;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Api.Services.Shared
{
    public class SceneService<T> where T : Scene
	{

		private readonly IBlobStorageService _blob;
		private readonly IContentRepository<T> _scenes;
		private readonly IContentRepository<Item> _items;
		private readonly IContentRepository<ItemDrop> _itemsDrop;

		public SceneService(
			IContentRepository<T> scenes, IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop, IBlobStorageService blob
		)
		{
			_scenes = scenes;
			_items = items;
			_itemsDrop = itemsDrop;
			_blob = blob;
		}

		public async Task<T> Add(T scene)
		{
			scene.Id = Guid.NewGuid();
			foreach (Item item in scene.Items)
			{
				item.SceneId = scene.Id;
				item.MediaUrl = await MoveFileIfTemp(scene, item.MediaUrl);
				if (item is ItemInput)
				{
					(item as ItemInput).Solution = CleanText((item as ItemInput).Solution);
				}
			}
			scene.BackgroundImage = await MoveFileIfTemp(scene, scene.BackgroundImage);
			scene.Thumbnail = await MoveFileIfTemp(scene, scene.Thumbnail);
			await _scenes.Add(scene);
			return await GetSingle(scene.Id);
		}

		public async Task<T> Delete(Guid id)
		{
			T scene = await GetSingle(id);
			RemoveAllImages(scene);
			await _scenes.Delete(scene);
			return scene;
		}

		public Task<List<T>> GetAll()
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetSingle(Guid id)
		{
			T scene = await _scenes
				.Query(new[] { "Items.Style", "Transition.TransitionProperties" })
				.AsNoTracking() // Because we use this method on update()
				.FirstOrDefaultAsync(e => e.Id == id);
			// FIXME:
			// Since .Net Core still does not support eager loading for relationships of derived 
			// classes (db.Items.Include("DragAnswers")), we have to load DragDrops in a different
			// query and join them manually. This is scheduled for the .net core 2.1 roadmap.
			// Follow this issue for more info: https://github.com/aspnet/EntityFrameworkCore/issues/3910
			List<ItemDrop> itemDropList = await _itemsDrop
				.Query(new[] { "DragAnswers" })
				.Where(d => d.SceneId == id)
				.AsNoTracking() // Because we use this method on update()
				.ToListAsync();
			foreach (ItemDrop itemDrop in itemDropList)
			{
				ItemDrop itemDropResult = scene.Items
					.Where(i => i.Id == itemDrop.Id)
					.Select(i => i as ItemDrop)
					.FirstOrDefault();
				itemDropResult.DragAnswers = itemDrop.DragAnswers;
			}
			RemoveRedundantDragDrops(scene.Items);
			scene.Items = scene.Items.OrderBy(i => i.ZIndex).ToList();
			return scene;
		}

		public async Task<T> Update(T scene)
		{
			List<Item> deleteItems = (await GetSingle(scene.Id)).Items;

			Guid[] itemDragIds = scene.Items.Where(i => i is ItemDrag).Select(d => d.Id).ToArray();
			foreach (Item item in scene.Items)
			{
				item.SceneId = scene.Id;
				item.MediaUrl = await MoveFileIfTemp(scene, item.MediaUrl);
				if (item is ItemInput)
				{
					(item as ItemInput).Solution = CleanText((item as ItemInput).Solution);
				}
				// Checking if each drag drop solution is related to a correct drag
				if (
					item is ItemDrop
					&& (item as ItemDrop).DragAnswers.Any(d => !itemDragIds.Contains(d.ItemDragId))
				) throw new HttpException(HttpStatusCode.BadRequest);
			}
			scene.BackgroundImage = await MoveFileIfTemp(scene, scene.BackgroundImage);
			scene.Thumbnail = await MoveFileIfTemp(scene, scene.Thumbnail);
			
			await _items.Delete(deleteItems);
			// Create new items
			await _items.Add(scene.Items);
			// By setting exercise.items to null we will avoid updating the exercises we just added
			// and so this will allegedly be more performant.
			scene.Items = null;
			await _scenes.Update(scene);
			return await GetSingle(scene.Id);
		}

		public void CleanForeignKeys(T scene)
		{
			// When an exercise is created from a template, or when an template is created from a exercise,
			// we have to delete all the references to the template/exercise (origin) ids in order to make
			// a full copy of the template and all its related tables. We also have to maintain the
			// relationship between drag and drop items giving them new guids.
			scene.Id = Guid.Empty;
			scene.Transition = null;
			List<Tuple<Guid, Guid>> ids = new List<Tuple<Guid, Guid>>();
			foreach (Item item in scene.Items)
			{
				item.Style.ItemId = item.Style.Id = item.SceneId = Guid.Empty;
				Guid newGuid = Guid.NewGuid();
				if (item.Type == ItemType.Drag || item.Type == ItemType.Drop)
					ids.Add(new Tuple<Guid, Guid>(item.Id, newGuid));
				item.Id = newGuid;
			}
			foreach (Item item in scene.Items.Where(i => i.Type == ItemType.Drop))
			{
				foreach (DragDrop dragdrop in ((ItemDrop)item).DragAnswers)
				{
					dragdrop.Id = Guid.NewGuid();
					dragdrop.ItemDragId = ids.First(i => i.Item1 == dragdrop.ItemDragId).Item2;
					dragdrop.ItemDropId = ids.First(i => i.Item1 == dragdrop.ItemDropId).Item2;
				}
			}
		}

		public async Task DuplicateImagesAsync(Scene scene, bool withThumbnail = true)
		{
			// TODO: if there are performance issues, we can call blobService in parallel.
			if (withThumbnail) scene.Thumbnail = await _blob.CopyFileByUrl(scene.Thumbnail, "temp/");
			scene.BackgroundImage = await _blob.CopyFileByUrl(scene.BackgroundImage, "temp/");
			foreach (Item item in scene.Items) item.MediaUrl = await _blob.CopyFileByUrl(item.MediaUrl, "temp/");
		}

		private void RemoveAllImages(Scene scene)
		{
			foreach (Item item in scene.Items)
			{
				_blob.DeleteFileByUrl(item.MediaUrl);
			}
			_blob.DeleteFileByUrl(scene.BackgroundImage);
			_blob.DeleteFileByUrl(scene.Thumbnail);
		}

		private void RemoveRedundantDragDrops(List<Item> items)
		{
			// Entity Framework will include the DragDrop virtuals in both ItemDrag and ItemDrop 
			// every time we have a join between Item - DragDrop. We do not want to send redundant 
			// data to the client or send multiple copies of DragDrop to update the database so we 
			// will remove the virtuals from ItemDrag manually, leaving the ones from ItemDrop.
			foreach (Item item in items)
			{
				if (item is ItemDrag) (item as ItemDrag).DropAnswers = null;
			}
		}

		private async Task<string> MoveFileIfTemp(T scene, string url)
		{
			if (string.IsNullOrEmpty(url)) return "";
			if (url.Contains("/temp/"))
			{
				string newUrl = scene.Id + "/";
				if (scene is Exercise)
				{
					newUrl = "activities/" + (scene as Exercise).ActivityId + "/" + newUrl;
				}
				else if (scene is Template)
				{
					newUrl = "templates/" + newUrl;
				}
				else
				{
					newUrl = "achievements/" + newUrl;
				}
				return await _blob.MoveFileByUrl(url, newUrl);
			}
			return url;
		}

		// Removes spaces in the begining and end of the text and replaces multiple spaces with a single one
		private string CleanText(string text)
		{
			string cleanString = text.Trim();
			return Regex.Replace(cleanString, @"\s+", " ");
		}
	}
}