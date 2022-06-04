using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;

namespace Hao.Authentication.Manager.DataMaps
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterM, CustomerInformation>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CustomerId, y => y.Ignore())
                .ForMember(x => x.Intro, y => y.MapFrom(z => "暂无"))
                .ForMember(x => x.Profession, y => y.MapFrom(z => "- - -"))
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now));

            CreateMap<CtmView, AuthCtmM>();

            CreateMap<SysRoleView, AuthRoleM>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Rank, y => y.MapFrom(z => z.Rank));
        }
    }
}
