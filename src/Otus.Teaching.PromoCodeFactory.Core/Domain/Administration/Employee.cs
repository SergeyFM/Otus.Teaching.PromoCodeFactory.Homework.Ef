namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
#nullable disable

public class Employee
    : BaseEntity {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string Email { get; set; }

    public Role Role { get; set; }

    public int AppliedPromocodesCount { get; set; }
}