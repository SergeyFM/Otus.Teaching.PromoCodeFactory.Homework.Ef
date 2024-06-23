namespace Otus.Teaching.PromoCodeFactory.WebHost.Models;
#nullable disable
public class PromoCodeResponse {
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string PartnerName { get; set; }
    public string ServiceInfo { get; set; }
}