namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

#nullable disable
public class CustomerPreference : BaseEntity {
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
    public Guid PreferenceId { get; set; }
    public Preference Preference { get; set; }
}
