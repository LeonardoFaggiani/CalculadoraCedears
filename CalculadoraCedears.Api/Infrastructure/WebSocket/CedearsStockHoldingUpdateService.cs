using CalculadoraCedears.Api.Dto.CedearsStockHolding;

using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CalculadoraCedears.Api.Infrastructure.WebSocket
{
    public interface ICedearsStockHoldingUpdateService
    {
        Task BroadcastCedearsStockHoldingUpdatesAsync(CedearsStockHoldingQueryResponse updates);
        void AddClient(string id, System.Net.WebSockets.WebSocket socket);
        void RemoveClient(string id);
    }

    public class CedearsStockHoldingUpdateService : ICedearsStockHoldingUpdateService
    {
        private readonly ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _clients = new();

        public async Task BroadcastCedearsStockHoldingUpdatesAsync(CedearsStockHoldingQueryResponse updates)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var message = JsonSerializer.Serialize(new
            {
                type = "cedears_stockholding_updated",
                data = JsonSerializer.Serialize(updates, options),
                timestamp = DateTime.Now                
            });

            var bytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients.Values)
            {
                if (client.State == WebSocketState.Open)
                {
                    try
                    {
                        await client.SendAsync(
                            new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending to client: {ex.Message}");
                    }
                }
            }
        }

        public void AddClient(string id, System.Net.WebSockets.WebSocket socket)
        {
            _clients.TryAdd(id, socket);
        }

        public void RemoveClient(string id)
        {
            _clients.TryRemove(id, out _);
        }
    }
}
