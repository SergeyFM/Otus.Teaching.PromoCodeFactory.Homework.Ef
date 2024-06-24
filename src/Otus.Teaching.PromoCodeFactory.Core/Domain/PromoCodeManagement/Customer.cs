﻿namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
#nullable disable
public class Customer
    : BaseEntity {
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public string Email { get; set; }

    public ICollection<CustomerPreference> CustomerPreferences { get; set; } = [];
    public ICollection<PromoCode> PromoCodes { get; set; } = [];
}