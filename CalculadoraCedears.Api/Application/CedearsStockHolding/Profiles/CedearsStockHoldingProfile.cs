using AutoMapper;

using CalculadoraCedears.Api.Dto;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Profiles
{
    public class CedearsStockHoldingProfile : Profile
    {
        public CedearsStockHoldingProfile()
        {
            CreateMap<Domain.CedearsStockHolding, CedearStockHoldingDto>(MemberList.None);

            CreateMap<UpdateCedearStockHoldingRequest, Domain.CedearsStockHolding>(MemberList.None);
        }
    }
}
