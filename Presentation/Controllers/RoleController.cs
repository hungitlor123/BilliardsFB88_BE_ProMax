using Application.Services.Interfaces;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/roles")]
[ApiController]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public IActionResult GetRoles([FromQuery] RoleFilterModel filter, [FromQuery] PaginationRequestModel pagination)
    {
        return _roleService.GetRoles(filter, pagination);
    }
    
}