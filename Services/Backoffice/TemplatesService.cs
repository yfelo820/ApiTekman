using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Entities.Content;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Api.Services.Shared;

namespace Api.Services.Backoffice
{
	public class TemplatesService: IApiService<Template>
	{
		private readonly IContentRepository<Template> _templates;
		private readonly IContentRepository<Item> _items;
		private readonly IContentRepository<ItemDrop> _itemsDrop;
		private readonly SceneService<Template> _sceneService;
		private readonly IContentRepository<Activity> _activities;
		
		public TemplatesService(
			IContentRepository<Template> templates, 
			IContentRepository<Item> items,
			IContentRepository<ItemDrop> itemsDrop,
			IContentRepository<Activity> activities,
			IBlobStorageService blob
		){
			_templates = templates;
			_items = items;
			_itemsDrop = itemsDrop;
			_activities = activities;
			_sceneService = new SceneService<Template>(_templates, _items, _itemsDrop, blob);
		}
			
		public async Task<Template> Add(Template template)
		{
			return await _sceneService.Add(template);
		}

		public async Task<Template> CopyFromExercise(Template template, Guid activityId)
		{
			Activity activity = await _activities.Get(activityId);
			template.SubjectId = activity.SubjectId;
			template.LanguageId = activity.LanguageId;
			_sceneService.CleanForeignKeys(template);
			await _sceneService.DuplicateImagesAsync(template);
			return await _sceneService.Add(template);
		}

		public async Task<Template> Delete(Guid id)
		{
			return await _sceneService.Delete(id);
		}

		public async Task<List<Template>> Filter(Guid subjectId, Guid languageId)
		{
			return await _templates.Query()
				.Where(e => e.SubjectId == subjectId && e.LanguageId == languageId)
				.OrderBy(e => e.Name)
				.ToListAsync();
		}

		public Task<List<Template>> GetAll()
		{
			return _sceneService.GetAll();
		}

		public async Task<Template> GetSingle(Guid id)
		{
			return await _sceneService.GetSingle(id);
		}

		public async Task<Template> Update(Template template)
		{
			return await _sceneService.Update(template);
		}

		public async Task<Template> CopyFromTemplate(Guid templateId)
		{
			Template template = await GetSingle(templateId);
			await _sceneService.DuplicateImagesAsync(template, withThumbnail: false);
			return template;
		}
	}
}