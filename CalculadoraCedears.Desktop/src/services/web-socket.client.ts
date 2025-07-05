import { getCurrentUser } from "@/lib/utils";
import { CedearsStockResponse } from "@/types/cedears";
import { UpdateCedearStockHoldingEvent } from "@/types/update-cedears-stock-holding-event";
import WebSocket, { Message } from "@tauri-apps/plugin-websocket";

class WebSocketClient {
  private socket: WebSocket | null = null;
  private listeners: any[] = [];
  private reconnectTimer: number | null = null;
  private reconnectAttempts = 0;
  private maxReconnectAttempts = 5;

  constructor(private url: string) {}

  async connect(): Promise<void> {
    if (this.socket) return;

    try {
      console.log(`Connecting to WebSocket: ${this.url}`);

      this.socket = await WebSocket.connect(this.url);

      console.log("WebSocket connection established");
      this.reconnectAttempts = 0;

      this.socket.addListener((event: Message) => {
        try {
          if (event.type === "Pong") {
          }

          if (event.type === "Close") {
            this.socket = null;

            if (this.reconnectAttempts < this.maxReconnectAttempts) {
              this.handleReconnect();
            }
          }

          if (event.type === "Text") {
            const eventResult: UpdateCedearStockHoldingEvent = JSON.parse(
              event.data
            );

            if (
              eventResult &&
              eventResult.type === "cedears_stockholding_updated"
            ) {
              const cedearsStockResponse: CedearsStockResponse = JSON.parse(
                eventResult.data
              );
              this.notifyListeners(cedearsStockResponse);
            }
          }
        } catch (error) {
          console.error("Error processing message:", error);
        }
      });
    } catch (error) {
      console.error("Failed to connect to WebSocket:", error);
      this.handleReconnect();
    }
  }

  private handleReconnect(): void {
    this.reconnectAttempts++;
    const delay = Math.min(1000 * Math.pow(2, this.reconnectAttempts), 30000);

    console.log(`Attempting to reconnect in ${delay}ms...`);

    if (this.reconnectTimer !== null) {
      clearTimeout(this.reconnectTimer);
    }

    this.reconnectTimer = window.setTimeout(() => {
      this.connect();
    }, delay);
  }

  async subscribe(listener: any): Promise<() => void> {
    this.listeners.push(listener);

    if (this.listeners.length === 1) {
      await getCurrentUser().then((user) => {
        this.url = `${this.url}?userId=${user.id}`;
        this.connect();
      });
    }

    return () => {
      this.listeners = this.listeners.filter((l) => l !== listener);

      if (this.listeners.length === 0) {
        this.disconnect();
      }
    };
  }

  private notifyListeners(cedearResponse: CedearsStockResponse): void {
    for (const listener of this.listeners) {
      listener(cedearResponse);
    }
  }

  async disconnect(): Promise<void> {
    if (this.socket) {
      await this.socket.disconnect();
      this.socket = null;
    }

    if (this.reconnectTimer !== null) {
      clearTimeout(this.reconnectTimer);
      this.reconnectTimer = null;
    }

    this.listeners = [];
  }
}

export const wsClient = new WebSocketClient("ws://localhost:5124/ws/stocks");