using FastPay.Server.Contracts.Employee;
using FastPay.Server.Services;
using FastPay.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastPay.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private EmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _employeeService = new EmployeeService();
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeRequest request)
        {

            Employee employee = new Employee
            {
                FirstName = request.firstName,
                LastName = request.lastName,
                CreatedAt = DateTime.Now,
                HourlyRate = request.hourlyRate
            };

            //Service writes to db
            try
            {
                await _employeeService.StoreEmployee(employee);
            
                return Ok() ;
            }
            catch
            {
                return NoContent();
            }
            
            
        }

    }
}
