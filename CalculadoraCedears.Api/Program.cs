using System.Reflection;

using MediatR;
using CalculadoraCedears.Api.Infrastructure.Extensions;
using CalculadoraCedears.Api.Infrastructure.WebSocket;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddRepositories();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDomainEvents();
builder.Services.AddBuilders();
builder.Services.AddConnectionString(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddBackgroundServices();
builder.Services.AddServices();

var app = builder.Build();

app.UseSwagger(builder.Environment);

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks();

app.UseWebSockets(new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
});

app.Use(async (context, next) =>
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

            // Enviar datos iniciales (opcional)
            //await SendInitialData(socket);

            // Mantener la conexión abierta y manejar mensajes entrantes

            var buffer = new byte[1024 * 4];
            var receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                // Aquí podrías procesar mensajes recibidos desde el cliente si es necesario
                // Por ejemplo, si el cliente quiere suscribirse solo a ciertos símbolos

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

app.UseHttpsRedirection();



app.Run();

