using AutoMapper;

using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Dto;

namespace CaluladoraCedears.Api.Application.Samples.Profiles
{
    public class CedearsQueryHandlerProfile : Profile
    {
        public CedearsQueryHandlerProfile()
        {
            CreateMap<Cedear, CedaerDto>(MemberList.None);
        }
    }
}
