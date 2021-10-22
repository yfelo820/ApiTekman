using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Backoffice.Multimedia;
using Api.DTO.Students;

namespace Api.Interfaces.Backoffice
{
    public interface IMultimediaService
    {
        Task<MultimediaResponse> GetSingle(Guid id);
        Task<Guid> Add(MultimediaRequest entity);
        Task Update(Guid id, MultimediaRequest multimedia);
        Task Delete(Guid id);
        Task<List<MultimediaListItem>> Filter(Guid subjectId, Guid languageId, MediaTypeRequest mediaType);

        Task<IEnumerable<MultimediaListItemResponse>> Get(
            string subjectKey, string languageKey,
            int course, MediaTypeRequest mediaTypeRequest);
    }
}