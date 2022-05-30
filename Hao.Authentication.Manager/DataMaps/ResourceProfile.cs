using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;

namespace Hao.Authentication.Manager.DataMaps
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<FileResource, ResourceM> ();
        }
    }
}
