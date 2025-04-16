using AutoMapper;

using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Application.Cedears.Profiles
{
    public class CedearsQueryHandlerProfile : Profile
    {
        public CedearsQueryHandlerProfile()
        {
            CreateMap<Cedear, CedaerDto>(MemberList.None);
        }
    }
}
