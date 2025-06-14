﻿using CalculadoraCedears.Api.Application.Controllers.Base;
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
        /// Autentifica el token de google, si es valido da de alta un usuario.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> PostLoginAsync([FromBody] UserRequest request, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new CreateUserCommand(request), cancellationToken));
        }

        /// <summary>
        /// Devuelve html de login exitoso.
        /// </summary>                
        /// <param name="cancellationToken"></param>
        [HttpGet]
        [Route("auth/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> PostAuthSuccessAsync(CancellationToken cancellationToken)
        {
            return Redirect($"/login-success.html");
        }
    }
}
