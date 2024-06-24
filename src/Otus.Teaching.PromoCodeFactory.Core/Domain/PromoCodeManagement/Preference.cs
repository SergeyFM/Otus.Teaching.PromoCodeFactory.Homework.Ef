namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
#nullable disable

public class Preference
    : BaseEntity {
    public string Name { get; set; }

    public string Description { get; set; }  // Новое поле, чтобы проверить работу миграций
    public ICollection<CustomerPreference> CustomerPreferences { get; set; }
}