using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;

namespace Hao.Authentication.Manager.DataMaps
{
    public class SysProfile : Profile
    {
        public SysProfile()
        {
            CreateMap<SysView, SysM>();
            CreateMap<SysM, Sys>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));

            CreateMap<Program, SysProgramM>()
                .ForMember(x => x.SysId, y => y.Ignore());

            CreateMap<SysCtmView, SysCtmM>()
                 .ForMember(x => x.Rank, y => y.MapFrom(z => z.RoleRank));

            CreateMap<SysRoleView, SysRoleM>();
            CreateMap<SysRoleM, SysRole>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
        }
    }
}
