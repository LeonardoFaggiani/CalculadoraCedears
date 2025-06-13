using CalculadoraCedears.Api.Application.CedearsStockHolding.Commands;
using CalculadoraCedears.Api.Application.CedearsStockHolding.Queries;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.CedearsStockHolding;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;

using MediatR;

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
        public async Task<IActionResult> PostAsync([FromBody] CedearStockHoldingRequest request, CancellationToken cancellationToken)
        {
            var query = new CedearStockHoldingCommand(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Devuelve todos los CedearsStockHolding activos por usuario.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Produces("application/json", Type = typeof(CedearsStockHoldingQueryResponse))]
        public async Task<IActionResult> GetAsync([FromQuery] string userId, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CedearsStockHoldingQuery(userId), cancellationToken));
        }

        /// <summary>
        /// Actualiza un CedearsStockHolding
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCedearStockHoldingRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCedearStockHoldingCommand(request);

            return Ok(await mediator.Send(command, cancellationToken));
        }

        /// <summary>
        /// Elimina un CedearsStockHolding
        /// </summary>        
        /// <param name="cedearsStockHoldingId"></param>
        /// <param name="cancellationToken"></param>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid cedearsStockHoldingId, CancellationToken cancellationToken)
        {
            var command = new DeleteCedearStockHoldingCommand(cedearsStockHoldingId);

            return Ok(await mediator.Send(command, cancellationToken));
        }
    }
}