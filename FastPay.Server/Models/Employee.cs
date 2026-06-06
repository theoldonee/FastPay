using FastPay.Server.Services.Interfaces;

namespace FastPay.Server.Models
{
    public class Employee : IEmployee
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public decimal HourlyRate { get; set; }
        public Guid Id { get; set ; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set ; }
        public DateTime UpdatedAt { get; set; }
    }
}
