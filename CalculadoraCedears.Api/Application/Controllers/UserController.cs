using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Application.Users.Commands;
using CalculadoraCedears.Api.Dto.CedearsStockHolding.Request;
using CalculadoraCedears.Api.Dto.Users.Request;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class UserController : ApiController
    {
        public UserController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Da de alta un usuario
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> PostLoginAsync([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CreateUserCommand(request), cancellationToken));
        }
    }
}
