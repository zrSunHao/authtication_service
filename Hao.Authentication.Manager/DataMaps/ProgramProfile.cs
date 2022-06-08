using AutoMapper;
using Hao.Authentication.Domain.Models;
using Hao.Authentication.Persistence.Entities;
using Hao.Authentication.Persistence.Views;

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
                .ForMember(x => x.ProgramId, y => y.MapFrom(z => z.PgmId))
                .ForMember(x => x.Category, y => y.Ignore())
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<ProgramSection, SectM>()
                .ForMember(x => x.PgmId, y => y.MapFrom(z => z.ProgramId));

            CreateMap<FunctM, ProgramFunction>()
                .ForMember(x => x.ProgramId, y => y.MapFrom(z => z.PgmId))
                .ForMember(x => x.SectionId, y => y.MapFrom(z => z.SectId))
                .ForMember(x => x.LastModifiedAt, y => y.MapFrom(z => DateTime.Now))
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.Now));
            CreateMap<ProgramFunction, FunctM>()
                .ForMember(x => x.PgmId, y => y.MapFrom(z => z.ProgramId))
                .ForMember(x => x.SectId, y => y.MapFrom(z => z.SectionId))
                .ForMember(x => x.CttMethod, y => y.Ignore())
                .ForMember(x => x.LimitedExpiredAt, y => y.Ignore());

            CreateMap<PgmFunctView, FunctM> ();

            CreateMap<CtmSimpleView, PgmCtmM>();
        }
    }
}
