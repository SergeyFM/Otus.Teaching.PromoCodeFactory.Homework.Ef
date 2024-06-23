using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Клиенты
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class CustomersController : ControllerBase {
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<PromoCode> _promoCodeRepository;

    public CustomersController(IRepository<Customer> customerRepository, IRepository<PromoCode> promoCodeRepository) {
        _customerRepository = customerRepository;
        _promoCodeRepository = promoCodeRepository;
    }

    /// <summary>
    /// Получение списка клиентов
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerShortResponse>>> GetCustomersAsync() {
        IEnumerable<Customer> customers = await _customerRepository.GetAllAsync();
        IEnumerable<CustomerShortResponse> response = customers.Select(c => new CustomerShortResponse {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email
        });

        return Ok(response);
    }

    /// <summary>
    /// Получение клиента по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id) {
        Customer? customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null) return NotFound();

        CustomerResponse response = new() {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
        };

        response.Preferences = customer.CustomerPreferences?.Select(cp => new PreferenceResponse {
            Id = cp.PreferenceId,
            Name = cp.Preference.Name
        }).ToList();
        response.PromoCodes = customer.PromoCodes?.Select(pc => new PromoCodeResponse {
            Id = pc.Id,
            Code = pc.Code,
            PartnerName = pc.PartnerName,
            ServiceInfo = pc.ServiceInfo
        }).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Создание нового клиента
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request) {
        Customer customer = new() {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        customer.CustomerPreferences = request.PreferenceIds.Select(pid => new CustomerPreference {
            CustomerId = customer.Id,
            PreferenceId = pid
        }).ToList();

        await _customerRepository.AddAsync(customer);

        return Ok(customer.Id);
    }

    /// <summary>
    /// Обновление данных клиента
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> EditCustomerAsync(Guid id, CreateOrEditCustomerRequest request) {
        Customer? customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null) return NotFound();

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.Email = request.Email;

        if (customer.CustomerPreferences is not null) {
            customer.CustomerPreferences.Clear();
            foreach (Guid pid in request.PreferenceIds) {
                customer.CustomerPreferences.Add(new CustomerPreference {
                    CustomerId = customer.Id,
                    PreferenceId = pid
                });
            }
        }
        await _customerRepository.UpdateAsync(customer);

        return Ok(customer.Id);
    }

    /// <summary>
    /// Удаление клиента
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomerAsync(Guid id) {
        Customer? customer = await _customerRepository.GetByIdAsync(id);
        if (customer is null) return NotFound();

        ICollection<PromoCode> promoCodes = customer?.PromoCodes ?? [];
        foreach (PromoCode promoCode in promoCodes)
            await _promoCodeRepository.DeleteAsync(promoCode.Id);

        await _customerRepository.DeleteAsync(id);

        return Ok();
    }
}
