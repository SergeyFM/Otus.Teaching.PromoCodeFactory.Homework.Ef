namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
#nullable disable

public class Preference
    : BaseEntity {
    public string Name { get; set; }
    public ICollection<CustomerPreference> CustomerPreferences { get; set; }
}