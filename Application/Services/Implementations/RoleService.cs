using Application.Services.Interfaces;
using Common.Extensions;
using Data.Repositories.Interfaces;
using Data.UnitOfWork.Interfaces;
using Domain.Entities;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _roleRepository = unitOfWork.Role;
    }
    
    public IActionResult GetRoles(RoleFilterModel filter, PaginationRequestModel pagination)
    {
        var query = _roleRepository.GetAll();

        if (filter.Name != null)
        {
            query = query.Where(x => x.Name.Contains(filter.Name));
        }
        var totalRows = query.Count();
        var roles = query
            .OrderByDescending(x => x.CreateAt)
            .Paginate(pagination)
            .ToList();
        return new OkObjectResult(roles.ToPaged(pagination, totalRows));
    }
}