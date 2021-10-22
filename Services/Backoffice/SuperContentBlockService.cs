using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Content;
using Api.Interfaces.Shared;

namespace Api.Services.Backoffice
{
    public class SuperContentBlockService : IApiService<SuperContentBlock>
    {

        private readonly IContentRepository<SuperContentBlock> _superContentBlocks;

        public SuperContentBlockService(IContentRepository<SuperContentBlock> repository)
        {
            _superContentBlocks = repository;
        }

        public async Task<SuperContentBlock> GetSingle(Guid id)
        {
            return await _superContentBlocks.Get(id);
        }

        public async Task<List<SuperContentBlock>> GetAll()
        {
            return (await _superContentBlocks.GetAll()).OrderBy(c => c.Order).ToList();
        }

        public async Task<SuperContentBlock> Delete(Guid id)
        {
            SuperContentBlock superContentBlock = await GetSingle(id);
            await _superContentBlocks.Delete(superContentBlock);
            return superContentBlock;
        }

        public async Task<SuperContentBlock> Add(SuperContentBlock superContentBlock)
        {
            superContentBlock.Id = Guid.NewGuid();            
            return await _superContentBlocks.Add(superContentBlock);
        }

        public async Task<SuperContentBlock> Update(SuperContentBlock superContentBlock)
        {            
            await _superContentBlocks.Update(superContentBlock);
            return superContentBlock;
        }
    }
}
