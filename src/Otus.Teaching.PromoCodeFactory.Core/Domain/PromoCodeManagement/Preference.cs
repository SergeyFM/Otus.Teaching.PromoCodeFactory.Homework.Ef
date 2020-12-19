﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement
{
    public class Preference
        :BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        
        public virtual ICollection<CustomerPreference> CustomerPreferences { get; set; }
    }
}