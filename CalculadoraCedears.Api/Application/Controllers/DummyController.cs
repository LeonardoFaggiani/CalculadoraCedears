using CalculadoraCedears.Api.Application.Controllers.Base;

using CommunityToolkit.Diagnostics;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraCedears.Api.Application.Controllers
{
    public class DummyController : ApiController
    {
        private readonly IConfiguration configuration;
        public DummyController(IMediator mediator, IConfiguration configuration) : base(mediator)
        {
            Guard.IsNotNull(configuration, nameof(configuration));

            this.configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {

            var values = new Dictionary<string, string>
            {
                { "redirect_uri", this.configuration["GoogleClient:RedirectUri"] },
            };

            return Ok(values);
        }
    }
}
