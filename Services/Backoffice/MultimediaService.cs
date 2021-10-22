using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTO.Backoffice.Multimedia;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Exceptions;
using Api.Interfaces.Backoffice;
using Api.Interfaces.Shared;
using Api.Services.Shared;
using Api.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MediaType = Api.Entities.Content.MediaType;

namespace Api.Services.Backoffice
{
    public class MultimediaService : IMultimediaService
    {
        private readonly IContentRepository<Multimedia> _multimediaRepository;
        private readonly MultimediaSettings _multimediaSettings;
        private readonly IBlobStorageService _storageService;
        private readonly ISubjectsService _subjectsService;
        private readonly ILanguagesService _languagesService;
        private readonly ICoursesService _coursesService;

        public MultimediaService(
            IOptions<MultimediaSettings> options,
            IContentRepository<Multimedia> multimediaRepository,
            IBlobStorageService storageService, 
            ISubjectsService subjectsService, 
            ILanguagesService languagesService, 
            ICoursesService coursesService)
        {
            _multimediaRepository = multimediaRepository;
            _multimediaSettings = options.Value;
            _storageService = storageService;
            _subjectsService = subjectsService;
            _languagesService = languagesService;
            _coursesService = coursesService;
        }

        public async Task<MultimediaResponse> GetSingle(Guid id)
        {
            var entity = await _multimediaRepository
                             .Query()
                             .Include(m => m.Subject)
                             .Include(m => m.Language)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(a => a.Id == id) ??
                         throw new NotFoundException($"The multimedia with id {id} doesn't exist.");

            var multimediaUrlPath = $"{_storageService.Uri.AbsoluteUri}/{_multimediaSettings.FolderName}";
            return MultimediaResponse.Map(entity, multimediaUrlPath,
                CleanIfTemp(entity.FileName));
        }

        public async Task<Guid> Add(MultimediaRequest multimedia)
        {
            var entity = MultimediaRequest.ToEntity(multimedia);
            await _multimediaRepository.Add(entity);
            return entity.Id;
        }

        public async Task Update(Guid id, MultimediaRequest multimedia)
        {
            var entity = await _multimediaRepository.Query().FirstOrDefaultAsync(m => m.Id == id);
            entity.Title = multimedia.Title;
            entity.FileName = CleanIfTemp(entity.FileName);
            entity.CourseId = multimedia.CourseId;

            await _multimediaRepository.Update(entity);
        }

        public async Task Delete(Guid id)
        {
            await _multimediaRepository.Delete(id);
        }

        public async Task<List<MultimediaListItem>> Filter(Guid subjectId, Guid languageId, MediaTypeRequest mediaTypeRequest)
        {
            var mediaType = (MediaType)mediaTypeRequest;
            var multimedia = await _multimediaRepository.Query(new[] { "Course" })
                .Where(a => a.SubjectId == subjectId &&
                a.LanguageId == languageId &&
                a.Type == mediaType)
                .OrderBy(a => a.Course.Number)
                .ToListAsync();
            return multimedia.ConvertAll(MultimediaListItem.ToListItem);
        }

        public async Task<IEnumerable<MultimediaListItemResponse>> Get(
            string subjectKey, string languageKey,
            int course, MediaTypeRequest mediaTypeRequest)
        {
            var subjectId = (await _subjectsService.GetSingle(subjectKey)).Id;
            var languageId = (await _languagesService.GetSingle(languageKey)).Id;
            var courseId = (await _coursesService.GetSingle(course)).Id;

            var mediaType = (MediaType)mediaTypeRequest;
            var multimedia = await _multimediaRepository.Query()
                .Where(m => m.SubjectId == subjectId &&
                m.LanguageId == languageId &&
                m.Type == mediaType &&
                m.CourseId == courseId)
                .OrderBy(m => m.Title)
                .ToListAsync();
            var multimediaUrlPath = $"{_storageService.Uri.AbsoluteUri}/{_multimediaSettings.FolderName}";
            return multimedia.Select(m => MultimediaListItemResponse.Map(m, multimediaUrlPath,
                CleanIfTemp(m.FileName)));
        }

        private string CleanIfTemp(string fileName)
        {
            return _storageService.RemoveTimestamp(fileName);
        }
    }
}