using AutoMapper;

using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Dto;

namespace CalculadoraCedears.Api.Application.Brokers.Profiles
{
    public class BrokerProfile : Profile
    {
        public BrokerProfile()
        {
            CreateMap<Broker, BrokerDto>(MemberList.None);
        }
    }
}
