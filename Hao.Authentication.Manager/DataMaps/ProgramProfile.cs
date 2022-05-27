using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Authentication.Manager.DataMaps
{
    public class ProgramProfile : Profile
    {
        public ProgramProfile()
        {
            CreateMap<ProgramM, Program>()
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<Program, ProgramM>();

            CreateMap<SectM, ProgramSection>()
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<ProgramSection, SectM>();

            CreateMap<FunctM, ProgramFunction>()
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<ProgramFunction, FunctM>();
        }
    }
}
