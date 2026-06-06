namespace FastPay.Server.Services.Interfaces
{
    public interface IEmployee
    {
         Guid Id {get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        decimal HourlyRate { get; set; }
        bool IsActive { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }

    }
}
