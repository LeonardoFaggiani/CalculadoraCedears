using CalculadoraCedears.Api.Application.Brokers.Queries;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.Brokers;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class BrokerController : ApiController
    {
        public BrokerController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Devuelve todos los brokers
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]        
        [Produces("application/json", Type = typeof(BrokerQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new BrokerQuery(), cancellationToken));
        }
    }
}
