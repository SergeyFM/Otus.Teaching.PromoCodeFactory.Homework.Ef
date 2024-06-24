using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Промокоды
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PromocodesController : ControllerBase {
    private readonly IRepository<PromoCode> _promoCodeRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Preference> _preferenceRepository;
    private readonly IRepository<CustomerPreference> _customerPreferenceRepository;

    public PromocodesController(IRepository<PromoCode> promoCodeRepository,
                                IRepository<Customer> customerRepository,
                                IRepository<Preference> preferenceRepository,
                                IRepository<CustomerPreference> customerPreferenceRepository) {
        _promoCodeRepository = promoCodeRepository;
        _customerRepository = customerRepository;
        _preferenceRepository = preferenceRepository;
        _customerPreferenceRepository = customerPreferenceRepository;
    }

    /// <summary>
    /// Получить все промокоды
    /// </summary>
    /// <returns>Список промокодов</returns>
    [HttpGet]
    public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync() {
        IEnumerable<PromoCode> promoCodes = await _promoCodeRepository.GetAllAsync();
        List<PromoCodeShortResponse> response = promoCodes.Select(pc => new PromoCodeShortResponse {
            Id = pc.Id,
            Code = pc.Code,
            ServiceInfo = pc.ServiceInfo,
            BeginDate = pc.BeginDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            EndDate = pc.EndDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            PartnerName = pc.PartnerName
        }).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Создать промокод и выдать его клиентам с указанным предпочтением
    /// </summary>
    /// <param name="request">Данные для создания промокода</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request) {
        IEnumerable<Preference> preferences = await _preferenceRepository.GetAllAsync();
        Preference? selectedPreference = preferences.FirstOrDefault(p => p.Name == request.Preference);
        if (selectedPreference is null)
            return BadRequest($"Предпочтение {request.Preference} не найдено.");

        IEnumerable<CustomerPreference> customerPreferences = await _customerPreferenceRepository.GetAllAsync();
        List<Guid> customerIDsWithPreference = customerPreferences
            .Where(cp => cp.PreferenceId == selectedPreference.Id)
            .Select(cp => cp.CustomerId)
            .ToList();

        if (customerIDsWithPreference.Count == 0)
            return BadRequest($"Нет пользователей с предпочтением {request.Preference}.");

        foreach (Guid customerID in customerIDsWithPreference) {
            Customer? customer = await _customerRepository.GetByIdAsync(customerID);
            if (customer is null) continue;

            PromoCode promoCode = new() {
                Id = Guid.NewGuid(),
                Code = $"{request.PromoCode}-{customerID}",
                BeginDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1),
                ServiceInfo = request.ServiceInfo,
                PartnerName = request.PartnerName,
                PreferenceId = selectedPreference.Id,
                CustomerId = customerID,
                EmployeeId = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f") //TODO: Implement Employee/Partner functionality
            };

            await _promoCodeRepository.AddAsync(promoCode);
            customer.PromoCodes.Add(promoCode);
            await _customerRepository.UpdateAsync(customer);
        }

        return Ok();
    }
}
