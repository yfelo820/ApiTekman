using System;
using System.Threading.Tasks;
using Api.Entities.Content;
using Api.Services.Backoffice;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Backoffice
{
    public class CoursesController : BaseBackofficeController
    {
        private readonly ICoursesService _service;
		public CoursesController(ICoursesService service) => _service = service;

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
        public async Task<IActionResult> Post([FromBody] Course course)
        {
            return Ok(await _service.Add(course));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Course course)
        {
            return Ok(await _service.Update(course));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await _service.Delete(id));
        }
    }
}
