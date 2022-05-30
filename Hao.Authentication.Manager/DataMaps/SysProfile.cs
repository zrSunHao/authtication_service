using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            CreateMap<SysCtmView, SysCtmM>();

            CreateMap<SysRoleView, SysRoleM>();
            CreateMap<SysRoleM, SysRole>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
        }
    }
}
