using CalculadoraCedears.Api.Infrastructure.WebSocket;

namespace CalculadoraCedears.Api.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationBuilder AddSecretEnviroment(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false)
                .AddJsonFile($"secret.settings.json", optional: true)
                .AddEnvironmentVariables();

            return builder;
        }

        public static WebApplication UseWebSockets(this WebApplication application)
        {
            application.UseWebSockets(new WebSocketOptions
             {
                 KeepAliveInterval = TimeSpan.FromMinutes(2)
             });

            application.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws/stocks")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var service = context.RequestServices.GetRequiredService<ICedearsStockHoldingUpdateService>();
                        var socket = await context.WebSockets.AcceptWebSocketAsync();
                        var clientId = Guid.NewGuid().ToString();


                        var userId = context.Request.Query["userId"].ToString();

                        service.AddClient(userId, socket);

                        var buffer = new byte[1024 * 4];
                        var receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        while (!receiveResult.CloseStatus.HasValue)
                        {
                            receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        }

                        await socket.CloseAsync(
                            receiveResult.CloseStatus.Value,
                            receiveResult.CloseStatusDescription,
                            CancellationToken.None);

                        service.RemoveClient(clientId);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

            return application;
        }
    }
}
