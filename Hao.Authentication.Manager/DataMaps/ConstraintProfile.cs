using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;

namespace Hao.Authentication.Manager.DataMaps
{
    public class ConstraintProfile : Profile
    {
        public ConstraintProfile()
        {
            CreateMap<CttView, CttM>();

            CreateMap<CttAddM,Constraint>()
                .ForMember(x=>x.Id,y=>y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));

        }
    }
}
