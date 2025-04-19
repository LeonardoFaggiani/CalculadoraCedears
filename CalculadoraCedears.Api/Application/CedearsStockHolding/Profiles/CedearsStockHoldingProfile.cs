using AutoMapper;

using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Application.CedearsStockHolding.Profiles
{
    public class CedearsStockHoldingProfile : Profile
    {
        public CedearsStockHoldingProfile()
        {
            CreateMap<Domain.CedearsStockHolding, CedearStockHoldingDto>(MemberList.None);
        }
    }
}
