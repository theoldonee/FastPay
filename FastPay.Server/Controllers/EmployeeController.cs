using FastPay.Server.Contracts.Employee;
using FastPay.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace FastPay.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
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

            
            return Ok() ;
            
        }

    }
}
