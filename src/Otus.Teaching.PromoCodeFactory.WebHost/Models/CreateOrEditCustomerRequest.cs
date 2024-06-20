namespace Otus.Teaching.PromoCodeFactory.WebHost.Models;
#nullable disable
public class CreateOrEditCustomerRequest {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public List<Guid> PreferenceIds { get; set; }
}