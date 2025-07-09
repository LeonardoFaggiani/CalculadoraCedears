using CalculadoraCedears.Api.Application.Cedears.Queries;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.Cedears;

using MediatR;

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
        [Produces("application/json", Type = typeof(CedearsQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CedearsQuery(), cancellationToken));
        }

        /// <summary>
        /// Devuelve la cotizacion del dolar CCL
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("ccl-quote")]
        [Produces("application/json", Type = typeof(DollarCCLQueryResponse))]
        public async Task<IActionResult> GetCurrentDollarCCLQuoteAsync(CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new DollarCCLQuery(), cancellationToken));
        }
    }
}