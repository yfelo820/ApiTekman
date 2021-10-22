using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Interfaces.Shared;
using Api.Entities.Content;
using System;
using System.Linq;

namespace Api.Services.Backoffice
{
	public class LanguagesService: ILanguagesService
	{
		private readonly IContentRepository<Language> _languages;
		public LanguagesService(IContentRepository<Language> repository) => _languages = repository;

		public async Task<List<Language>> GetAll()
		{
			return (await _languages.GetAll()).OrderBy(c => c.Name).ToList();
		}
	
		public async Task<Language> GetSingle(Guid id)
		{
			return await _languages.Get(id);
		}

        public async Task<Language> GetSingle(string code)
        {
            return await _languages.FindSingle(l => string.Equals(l.Code, code));
        }

        public async Task<Language> Add(Language language)
		{
			return await _languages.Add(language);
		}

		public async Task<Language> Update(Language language)
		{
			await _languages.Update(language);
			return language;
		}

		public async Task<Language> Delete(Guid id) {
			Language language = await GetSingle(id);
			await _languages.Delete(language);
			return language;
		}
    }
}