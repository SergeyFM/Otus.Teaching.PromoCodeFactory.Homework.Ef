using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Контроллер для получения списка предпочтений
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PreferencesController(
    IRepository<Preference> _preferenceRepository
    ) : ControllerBase {

    /// <summary>
    /// Получение списка предпочтений
    /// </summary>
    /// <returns>Список предпочтений</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PreferenceResponse>>> GetPreferencesAsync() {
        IEnumerable<Preference> preferences = await _preferenceRepository.GetAllAsync();
        List<PreferenceResponse> response = preferences.Select(p => new PreferenceResponse {
            Id = p.Id,
            Name = p.Name
        }).ToList();

        return Ok(response);
    }
}
