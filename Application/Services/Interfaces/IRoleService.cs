using Domain.Entities;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces;

public interface IRoleService
{
     IActionResult GetRoles(RoleFilterModel filter, PaginationRequestModel pagination);
}