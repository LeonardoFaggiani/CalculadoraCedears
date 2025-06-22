using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Application.Users.Commands;
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
        /// Autentifica el token de google, si es valido da de alta un usuario y rediriji a login exitoso.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("auth/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> PostAuthSuccessAsync([FromQuery] string code, [FromQuery] string state, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(code))            
                return BadRequest("No se recibió el código de autorización.");            

            var request = new UserRequest()
            {
                GoogleToken = code,
            };

            var userResponse = await mediator.Send(new CreateUserCommand(request), cancellationToken);

            return Redirect($"/login-success.html?token={userResponse.Jwt}");
        }
    }
}
