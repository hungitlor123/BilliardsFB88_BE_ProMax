using AutoMapper;
using Domain.Entities;
using Domain.Models.Creates;
using Domain.Models.Updates;
using Domain.Models.View;

namespace Application.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Role, RoleViewModel>();
            // .ForMember(dest => dest.Ten, opt => opt.MapFrom(src => src.Name))
            // //bỏ qua thuộc tính không cần map
            // .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => src.CreateAt));

        CreateMap<RoleCreateModel, Role>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(scr => Guid.NewGuid()));

        CreateMap<RoleUpdateModel, Role>()
            .ForAllMembers(opts => opts.Condition((_, _, scrMember) => scrMember != null));

    }
    
}