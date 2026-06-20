using System.ComponentModel.DataAnnotations;

namespace FastPay.Server.Contracts.Employee
{
    public class UpdateEmployeeContract
    {
        public Guid Id { get; }
        public bool isActive { get; set; }


        [Range(0.01, double.MaxValue)]
        public decimal hourlyRate { get; set; }
    }
}
