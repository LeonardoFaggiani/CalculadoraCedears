using CaluladoraCedears.Api.Application.Controllers.Base;
using CaluladoraCedears.Api.Application.Samples.Commands;
using CaluladoraCedears.Api.Application.Samples.Queries;
using CaluladoraCedears.Api.Dto.Samples;
using CaluladoraCedears.Api.Dto.Samples.Request;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaluladoraCedears.Api.Application.Controllers
{
    public class CedearsController : ApiController
    {
        public CedearsController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Get all cedears
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json", Type = typeof(CedearsQueryResponse))]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var query = new CedearsQuery();

            return Ok(await mediator.Send(query, cancellationToken));
        }

        /// <summary>
        /// Create a Cedear
        /// </summary>        
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostCedearAsync([FromBody] CreateCedearRequest request, CancellationToken cancellationToken)
        {
            var query = new CreateCedearCommand(request);

            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}