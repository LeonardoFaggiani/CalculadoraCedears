using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Application.Cedears.Profiles
{
    public class CedearsProfile : Profile
    {
        public CedearsProfile()
        {
            CreateMap<Cedear, CedearDto>(MemberList.None);                
        }
    }
}
