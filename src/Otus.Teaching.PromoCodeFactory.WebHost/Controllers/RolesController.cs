﻿using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Роли сотрудников
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class RolesController {
    private readonly IRepository<Role> _rolesRepository;

    public RolesController(IRepository<Role> rolesRepository) => _rolesRepository = rolesRepository;

    /// <summary>
    /// Получить все доступные роли сотрудников
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IEnumerable<RoleItemResponse>> GetRolesAsync() {
        IEnumerable<Role> roles = await _rolesRepository.GetAllAsync();

        List<RoleItemResponse> rolesModelList = roles.Select(x =>
            new RoleItemResponse() {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();

        return rolesModelList;
    }
}