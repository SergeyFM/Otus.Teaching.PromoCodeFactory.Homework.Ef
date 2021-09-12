﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories.EntityFrameworkRepositories
{
    public class PreferencesEntityFrameworkRepository : EntityFrameworkRepository<Preference>, IPreferencesRepository
    {
        public PreferencesEntityFrameworkRepository(PromoCodeDataContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Preference> GetByIdAsync(Guid id)
        {
            var item = await DbContext.Preferences
                .Where(x => x.Id == id)
                .Include(x => x.CustomerPreferences)
                .ThenInclude(x => x.Customer)
                .FirstOrDefaultAsync();

            return item;
        }

        public Task<Preference> GetPreferenceByName(string preferenceName)
        {
            return DbContext.Preferences
                .FirstOrDefaultAsync(x => x.Name == preferenceName);
        }
    }
}