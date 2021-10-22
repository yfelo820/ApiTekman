using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Entities.Content;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Backoffice
{
	public class ContentBlocksService: IApiService<ContentBlock>
	{
		private readonly IBlobStorageService _blob;
		private readonly IContentRepository<ContentBlock> _contentBlocks;

		private readonly string ContentBlocksUrl = "content-blocks";

		public ContentBlocksService(
			IContentRepository<ContentBlock> repository,
			IBlobStorageService blob
		) {
			_contentBlocks = repository;
			_blob = blob;
		}

		public async Task<List<ContentBlock>> Filter(Guid languageId, Guid subjectId)
		{
			return await _contentBlocks.Query()
				.Where(c => c.LanguageId == languageId && c.SubjectId == subjectId)
				.OrderBy(c => c.Order)
				.ToListAsync();
		}
	
		public async Task<ContentBlock> GetSingle(Guid id)
		{
			return await _contentBlocks.Get(id);
		}

		public async Task<ContentBlock> Add(ContentBlock contentBlock)
		{
			contentBlock.Id = Guid.NewGuid();
			contentBlock.Image = await MoveImageFromTemp(contentBlock);
			return await _contentBlocks.Add(contentBlock);
		}

		public async Task<ContentBlock> Update(ContentBlock contentBlock)
		{
			if (contentBlock.Image.Contains("/temp/")) {
				contentBlock.Image = await MoveImageFromTemp(contentBlock);
			}
			await _contentBlocks.Update(contentBlock);
			return contentBlock;
		}

		public async Task<ContentBlock> Delete(Guid id) {
			ContentBlock contentBlock = await GetSingle(id);
			_blob.DeleteFileByUrl(contentBlock.Image);
			await _contentBlocks.Delete(contentBlock);
			return contentBlock;
		}
		
		private async Task<string> MoveImageFromTemp (ContentBlock contentBlock) {
			string newUrl = ContentBlocksUrl + "/" + contentBlock.Id + "/";
			return await _blob.MoveFileByUrl(contentBlock.Image, newUrl);
		}

		public async Task<List<ContentBlock>> GetAll()
		{
			return (await _contentBlocks.GetAll()).OrderBy(c => c.Order).ToList();
		}
	}
}