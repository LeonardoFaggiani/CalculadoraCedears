using CalculadoraCedears.Api.Dto.CedearsStockHolding;

using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CalculadoraCedears.Api.Infrastructure.WebSocket
{
    public interface ICedearsStockHoldingUpdateService
    {
        Task BroadcastCedearsStockHoldingUpdatesAsync(System.Net.WebSockets.WebSocket client, CedearsStockHoldingQueryResponse updates);
        void AddClient(string id, System.Net.WebSockets.WebSocket socket);
        void RemoveClient(string id);
        ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> Clients { get; }
    }

    public class CedearsStockHoldingUpdateService : ICedearsStockHoldingUpdateService
    {
        public ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> Clients { get; set; }

        public CedearsStockHoldingUpdateService()
        {
            this.Clients = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();           
        }

        public async Task BroadcastCedearsStockHoldingUpdatesAsync(System.Net.WebSockets.WebSocket client, CedearsStockHoldingQueryResponse updates)
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

        public void AddClient(string id, System.Net.WebSockets.WebSocket socket)
        {
            if (this.Clients.ContainsKey(id))
            {
                RemoveClient(id);
                AddClient(id, socket);
                return;
            }

            this.Clients.TryAdd(id, socket);
        }

        public void RemoveClient(string id)
        {
            this.Clients.TryRemove(id, out _);
        }
    }
}
