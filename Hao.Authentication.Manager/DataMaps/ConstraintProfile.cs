using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Views;

namespace Hao.Authentication.Manager.DataMaps
{
    public class ConstraintProfile : Profile
    {
        public ConstraintProfile()
        {
            CreateMap<CttView, CttM>();
        }
    }
}
