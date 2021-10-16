using System;
using System.Threading.Tasks;
using Core.Models;
using Core.Processors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeProcessor _employeeProcessor;

        public EmployeeController(EmployeeProcessor employeeProcessor)
        {
            _employeeProcessor = employeeProcessor;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var employee = await _employeeProcessor.FindEmployee(id);
            if (employee == null)
                return NoContent();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            return StatusCode(501);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Employee employee)
        {
            return StatusCode(501);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return StatusCode(501);
        }
    }
}
