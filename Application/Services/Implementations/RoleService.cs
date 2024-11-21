using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Extensions;
using Data.Repositories.Interfaces;
using Data.UnitOfWork.Interfaces;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _autoMapper;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _roleRepository = unitOfWork.Role;
        _autoMapper = mapper;
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
            
            //map thu cong
            // .Select(x => new RoleViewModel()
            // {
            //     Id = x.Id,
            //     Ten = x.Name,
            //     NgayTao  = x.CreateAt
            // })
            //
            //auto mapper
            .ProjectTo<RoleViewModel>(_autoMapper.ConfigurationProvider)
            
            
            .ToList();
        return new OkObjectResult(roles.ToPaged(pagination, totalRows));
    }

    public IActionResult GetRoleById(Guid id)
    {
        var role = _roleRepository.Where(x => x.Id.Equals(id))
            .ProjectTo<RoleViewModel>(_autoMapper.ConfigurationProvider)
            .FirstOrDefault();

        if (role == null)
        {
            return new NotFoundResult();
        }

        return new ObjectResult(role);
    }

    public async Task<IActionResult> CreateRole(RoleCreateModel model)
    {
        // var role = new Role()
        // {
        //     Id = Guid.NewGuid(),
        //     Name = model.Name,
        //     CreateAt = DateTime.UtcNow
        // };
        
        var role = _autoMapper.Map<Role>(model);
        
        _roleRepository.Add(role);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result > 0)
        {
            return GetRoleById(role.Id);
        }

        return new BadRequestResult();
    }

    public async Task<IActionResult> UpdateRole(Guid id, RoleUpdateModel model)
    {
        var role = await _roleRepository.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        if (role == null)
        {
            return new NotFoundResult();
        }

        var update = _autoMapper.Map(model, role);
        
        _roleRepository.Update(update);
        
        var result = await _unitOfWork.SaveChangesAsync();
        if (result > 0)
        {
            return GetRoleById(role.Id);
        }

        return new BadRequestResult();
    }

    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var role = _roleRepository.Where(x => x.Id.Equals(id)).FirstOrDefault();
        if (role == null)
        {
            return new NotFoundResult();
        }
        
        _roleRepository.Delete(role);
        await _unitOfWork.SaveChangesAsync();
        return new NoContentResult();
    }
}