using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;

namespace Hao.Authentication.Manager.DataMaps
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<FileResource, ResourceM>();
            CreateMap<ResourceM, FileResource>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
        }
    }
}
