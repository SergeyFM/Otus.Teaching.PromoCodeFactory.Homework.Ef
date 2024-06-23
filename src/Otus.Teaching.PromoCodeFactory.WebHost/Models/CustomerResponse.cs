namespace Otus.Teaching.PromoCodeFactory.WebHost.Models;
#nullable disable

public class CustomerResponse {
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public List<PreferenceResponse> Preferences { get; set; }
    public List<PromoCodeResponse> PromoCodes { get; set; }
}