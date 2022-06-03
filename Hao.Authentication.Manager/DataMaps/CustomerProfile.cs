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
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CtmView, CtmM > ();
            CreateMap<CustomerInformation, PeopleM>()
                .ForMember(x => x.CtmId, y => y.MapFrom(z => z.CustomerId));

            CreateMap<CtmRoleView, CtmRoleM>();

            CreateMap<CtmCttAddM, CttAddM>()
                .ForMember(x => x.TargetId, y => y.MapFrom(z => z.CtmId))
                .ForMember(x => x.Origin, y => y.MapFrom(z => "管理员添加"));
        }
    }
}
