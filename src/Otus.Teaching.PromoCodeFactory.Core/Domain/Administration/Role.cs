namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
#nullable disable

public class Role
    : BaseEntity {
    public string Name { get; set; }

    public string Description { get; set; }
}