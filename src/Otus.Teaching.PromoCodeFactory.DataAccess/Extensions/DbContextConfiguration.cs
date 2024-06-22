using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Extensions;
public static class DbContextConfiguration {
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, string cnn) {
        Console.WriteLine("ConfigureDbContext...");
        services.AddDbContext<DatabaseContext>(options => {
            options
                .UseSqlite(cnn)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        });

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>)); // srazu for all types

        // Recreate and seed the db
        using (ServiceProvider serviceProvider = services.BuildServiceProvider()) {
            using DatabaseContext context = serviceProvider.GetRequiredService<DatabaseContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            SeedDatabase(context);
        }

        return services;
    }

    private static void SeedDatabase(DatabaseContext context) {
        Console.WriteLine("SeedDatabase...");
        // Seed Employees AND Roles
        List<Employee> employees = FakeDataFactory.Employees.ToList();
        context.Employees.AddRange(employees);
        context.SaveChanges();

        // Seed Preferences
        List<Preference> preferences = FakeDataFactory.Preferences.ToList();
        context.Preferences.AddRange(preferences);
        context.SaveChanges();

        // Seed Customers
        List<Customer> customers = FakeDataFactory.Customers.ToList();
        context.Customers.AddRange(customers);
        context.SaveChanges();

        // Seed Customer Preferences
        foreach (Customer? customer in customers) {
            List<CustomerPreference> customerPreferences = FakeDataFactory.Preferences
                    .Select(p => new CustomerPreference {
                        CustomerId = customer.Id,
                        PreferenceId = p.Id
                    }).ToList();
            context.CustomerPreferences.AddRange(customerPreferences);
        }
        context.SaveChanges();

        // Seed PromoCodes
        List<PromoCode> promoCodes = FakeDataFactory.PromoCodes.ToList();
        foreach (PromoCode? promoCode in promoCodes) {
            promoCode.Preference = preferences.First(p => p.Id == promoCode.PreferenceId);
            promoCode.PartnerManager = employees.First(e => e.Id == promoCode.EmployeeId);
            promoCode.Customer = customers.First(c => c.Id == promoCode.CustomerId);
            context.PromoCodes.Add(promoCode);
        }
        context.SaveChanges();
    }
}

