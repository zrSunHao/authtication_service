using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;

namespace Hao.Authentication.Manager.DataMaps
{
    public class LogProfile: Profile
    {
        public LogProfile()
        {
            CreateMap<LogM, CustomerLog>()
                .ForMember(x => x.Id, y => y.Ignore());
        }
    }
}
