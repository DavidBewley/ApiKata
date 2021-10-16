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
        public IActionResult Get()
        {
            var employee = _employeeProcessor.FindEmployees();
            if (employee == null)
                return NoContent();

            return Ok(employee);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var employee = _employeeProcessor.FindEmployee(id);
            if (employee == null)
                return NoContent();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Name) || employee.StartDate == DateTime.MinValue)
                return BadRequest("Name or start date is missing");

            var createdEmployee = _employeeProcessor.CreateEmployee(employee);

            return Ok(createdEmployee);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Employee employee)
        {
            if (string.IsNullOrEmpty(employee.Name) || employee.StartDate == DateTime.MinValue)
                return BadRequest("Name or start date is missing");

            var updateEmployee = _employeeProcessor.UpdateEmployee(employee);
            
            if(updateEmployee == null)
                return NotFound("Employee does not exist");

            return Ok(updateEmployee);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            var deleteEmployee = _employeeProcessor.DeleteEmployee(id);
            if (deleteEmployee == null)
                return NotFound("Employee does not exist");

            return Ok();
        }
    }
}
