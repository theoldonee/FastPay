using FastPay.Server.Services.Interfaces;

namespace FastPay.Server.Models
{
    public class Employee 
    {
        public string FirstName { get => _firstName; set => _firstName = value; }
        private string _firstName = string.Empty; 

        public string LastName { get => _lastName; set => _lastName = value; }
        private string _lastName = string.Empty;

        public decimal HourlyRate { get => _hourleyRate; set => _hourleyRate = value; }
        private decimal _hourleyRate;
        public Guid Id { get => _id; set => _id = value; }
        private Guid _id;
        public bool IsActive { get => _isActive; set => _isActive =  value; }
        private bool _isActive;
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        private DateTime _createdAt;
        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
        private DateTime _updatedAt;
    }
}
