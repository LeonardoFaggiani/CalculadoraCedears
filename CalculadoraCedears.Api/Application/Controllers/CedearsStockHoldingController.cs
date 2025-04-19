using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.CedearsStockHolding.Queries;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.CedearsStockHolding;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class CedearsStockHoldingController : ApiController
    {
        public CedearsStockHoldingController(IMediator mediator) : base(mediator)
        { }


        /// <summary>
        /// Crea un CedearsStockHolding
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromBody] CedearStockHoldingRequest request, CancellationToken cancellationToken)
        {
            var query = new CedearStockHoldingCommand(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Devuelve todos los CedearsStockHolding activos
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(CedearsStockHoldingQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CedearsStockHoldingQuery(), cancellationToken));
        }
    }
}