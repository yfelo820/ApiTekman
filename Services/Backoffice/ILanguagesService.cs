using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Entities.Content;

namespace Api.Services.Backoffice
{
    public interface ILanguagesService
    {
        Task<Language> GetSingle(Guid id);
        Task<Language> GetSingle(string code);
        Task<List<Language>> GetAll();
        Task<Language> Add(Language entity);
        Task<Language> Update(Language entity);
        Task<Language> Delete(Guid id);
    }
}