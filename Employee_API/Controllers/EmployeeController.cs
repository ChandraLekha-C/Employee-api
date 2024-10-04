using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Employee_API.Models.NewFolder;

namespace Employee_API.Controllers
{
    [Route("api/EmployeeAPI")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EmployeeDto>> GetEmployees()
        {
            _logger.LogInformation("Fetching all employees.");
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EmployeeDto> GetEmployeeById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided: {Id}", id);
                return BadRequest("Invalid ID.");
            }

            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
            {
                _logger.LogInformation("Employee with ID {Id} not found.", id);
                return NotFound();
            }

            _logger.LogInformation("Employee with ID {Id} retrieved successfully.", id);
            return Ok(emp);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<EmployeeDto> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                _logger.LogWarning("Received null employee DTO.");
                return BadRequest("Employee DTO is null.");
            }

            if (employeeDto.Employee_Id <= 0)
            {
                _logger.LogWarning("Invalid Employee ID: {Id}", employeeDto.Employee_Id);
                return BadRequest("Invalid Employee ID.");
            }

            var existingEmployee = _employeeService.GetEmployeeById(employeeDto.Employee_Id);
            if (existingEmployee != null)
            {
                _logger.LogWarning("Employee ID already exists: {Id}", employeeDto.Employee_Id);
                return BadRequest("Employee ID already exists.");
            }

            _employeeService.AddEmployee(employeeDto);
            _logger.LogInformation("Employee created with ID {Id}.", employeeDto.Employee_Id);

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeDto.Employee_Id }, employeeDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID provided for deletion: {Id}", id);
                return BadRequest("Invalid ID.");
            }

            var emp = _employeeService.GetEmployeeById(id);
            if (emp == null)
            {
                _logger.LogInformation("Employee with ID {Id} not found for deletion.", id);
                return NotFound("Employee not found.");
            }

            _employeeService.DeleteEmployee(id);
            _logger.LogInformation("Employee with ID {Id} deleted successfully.", id);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateEmployee(int id, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || id != employeeDto.Employee_Id)
            {
                _logger.LogWarning("Employee ID mismatch or request body is null for ID: {Id}", id);
                return BadRequest("Employee ID mismatch or request body is null.");
            }

            var existingEmployee = _employeeService.GetEmployeeById(id);
            if (existingEmployee == null)
            {
                _logger.LogInformation("Employee with ID {Id} not found for update.", id);
                return NotFound($"Employee with ID {id} not found.");
            }

            _employeeService.UpdateEmployee(id, employeeDto);
            _logger.LogInformation("Employee with ID {Id} updated successfully.", id);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdatePartialEmployee(int id, [FromBody] JsonPatchDocument<EmployeeDto> patchDto)
        {
            if (patchDto == null || id <= 0)
            {
                _logger.LogWarning("Patch document is null or invalid ID provided for ID: {Id}", id);
                return BadRequest("Invalid ID or patch document is null.");
            }

            try
            {
                _employeeService.UpdatePartialEmployee(id, patchDto);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound($"Employee with ID {id} not found.");
            }

            _logger.LogInformation("Employee with ID {Id} partially updated successfully.", id);

            return NoContent();
        }
    }
}
