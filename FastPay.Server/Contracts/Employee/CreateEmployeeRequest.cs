using System.ComponentModel.DataAnnotations;

namespace FastPay.Server.Contracts.Employee
{
    public class CreateEmployeeRequest
    {
        [Required]
        public string firstName;

        [Required]
        public string lastName;

        [Range (0.01, double.MaxValue )]
        public decimal hourlyRate;
    }
}
