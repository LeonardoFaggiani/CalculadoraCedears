using CalculadoraCedears.Api.Application.Cedears.Commands;
using CalculadoraCedears.Api.Application.Cedears.Queries;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.Cedears;
using CalculadoraCedears.Api.Dto.Cedears.Request;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class CedearsController : ApiController
    {
        public CedearsController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Devuelve todos los cedears donde coindica el ticker
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]
        [Route("search-by-ticker")]
        [Produces("application/json", Type = typeof(SearchCedearsByTickerQueryResponse))]
        public async Task<IActionResult> GetSearchByTickerAsync([FromQuery] SearchCedearsByTickerQuery searchCedearsQuery, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(searchCedearsQuery, cancellationToken));
        }

        /// <summary>
        /// Create a Cedear
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostCedearAsync([FromBody] CedearStockHoldingRequest request, CancellationToken cancellationToken)
        {
            var query = new CedearStockHoldingCommand(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Devuelve todos los cedears activos
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]        
        [Produces("application/json", Type = typeof(CedearsQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CedearsQuery(), cancellationToken));
        }
    }
}