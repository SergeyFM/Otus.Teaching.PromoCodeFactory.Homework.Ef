﻿using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

public class PromoCode
    : BaseEntity {
    public string Code { get; set; }

    public string ServiceInfo { get; set; }

    public DateTime BeginDate { get; set; }

    public DateTime EndDate { get; set; }

    public string PartnerName { get; set; }

    public Guid EmployeeId { get; set; } // Foreign key
    public Employee PartnerManager { get; set; }
    public Guid PreferenceId { get; set; } // Foreign key
    public Preference Preference { get; set; }
    public Guid CustomerId { get; set; } // Foreign key
    public Customer Customer { get; set; }
}