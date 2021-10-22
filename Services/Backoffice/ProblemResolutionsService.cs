using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Content;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Backoffice
{
    public class ProblemResolutionsService : IApiService<ProblemResolution>
    {
        private readonly IContentRepository<ProblemResolution> _problemResolutions;
        private readonly ILanguagesService _languagesService;

        public ProblemResolutionsService(IContentRepository<ProblemResolution> repository,
            ILanguagesService languagesService)
        {
            _problemResolutions = repository;
            _languagesService = languagesService;
        }

        public async Task<List<ProblemResolution>> Filter(Guid? languageId)
        {
            var problemResolutions = _problemResolutions.Query();

            if (languageId.HasValue)
            {
                problemResolutions = problemResolutions.Where(pr => pr.LanguageId == languageId);
            }

            return await problemResolutions
                .OrderBy(pr => pr.Name)
                .ToListAsync();
        }

        public async Task<List<ProblemResolution>> Get(string languageKey)
        {
            var languageId = (await _languagesService.GetSingle(languageKey)).Id;
            var problemResolutions = _problemResolutions.Query();

            problemResolutions = problemResolutions.Where(pr => pr.LanguageId == languageId);

            return await problemResolutions
                .OrderBy(pr => pr.Name)
                .ToListAsync();
        }

        public async Task<ProblemResolution> GetSingle(Guid id)
        {
            return await _problemResolutions.Get(id);
        }

        public async Task<List<ProblemResolution>> GetAll()
        {
            return (await _problemResolutions.GetAll()).OrderBy(c => c.Name).ToList();
        }

        public async Task<ProblemResolution> Add(ProblemResolution entity)
        {
            return await _problemResolutions.Add(entity);
        }

        public async Task<ProblemResolution> Update(ProblemResolution entity)
        {
            await _problemResolutions.Update(entity);
            return entity;
        }

        public async Task<ProblemResolution> Delete(Guid id)
        {
            var problemResolution = await GetSingle(id);
            await _problemResolutions.Delete(problemResolution);

            return problemResolution;
        }
    }
}
