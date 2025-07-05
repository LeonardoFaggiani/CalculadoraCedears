using CalculadoraCedears.Api.Application.Auth.Commands;
using CalculadoraCedears.Api.Application.Controllers.Base;
using CalculadoraCedears.Api.Dto.Auth.Request;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class AuthController : ApiController
    {
        public AuthController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Autentifica el token de google, si es valido da de alta un usuario y rediriji a login exitoso.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("callback")]
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

            return Redirect($"/login-success.html?token={Uri.EscapeDataString(userResponse.Jwt)}&refreshToken={Uri.EscapeDataString(userResponse.RefreshToken)}");
        }

        /// <summary>
        /// Valida el refreshToken y genera un nuevo jwt, si el refreshToken vencio, se desloguea al usaurio.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> PostRefreshTokenAsync([FromBody] JwtTokenRequest jwtTokenRequest, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CreateJwtTokenCommand(jwtTokenRequest), cancellationToken));
        }

        /// <summary>
        /// Desloguea el usuario.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> PostLogoutAsync([FromBody] LogOutUserRequest logOutUserRequest, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new LogOutUserCommand(logOutUserRequest), cancellationToken));
        }
    }
}
