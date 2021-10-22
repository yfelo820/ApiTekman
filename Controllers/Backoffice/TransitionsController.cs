using System.Threading.Tasks;
using Api.Entities.Content;
using Microsoft.AspNetCore.Mvc;
using Api.Interfaces.Shared;
using System;

namespace Api.Controllers.Backoffice
{
	public class TransitionsController : BaseBackofficeController
    {   
        private readonly IApiService<Transition> _service;

		public TransitionsController(IApiService<Transition> service) => _service = service;

		[HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _service.GetSingle(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transition transition)
        {
            return Ok(await _service.Add(transition));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Transition transition)
        {
            return Ok(await _service.Update(transition));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
