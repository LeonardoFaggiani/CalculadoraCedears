import WebSocket, { Message } from "@tauri-apps/plugin-websocket";

export type WebSocketEventHandler<T> = (data: T) => void;

export class WebSocketClient<T = any> {
  private socket: WebSocket | null = null;
  private listeners: WebSocketEventHandler<T>[] = [];
  private reconnectTimer: number | null = null;
  private reconnectAttempts = 0;
  private isConnecting:boolean = false;
  private readonly maxReconnectAttempts = 3;

  constructor(private url: string, private eventType: string) { }

  async connect(): Promise< WebSocket | null > {
  if (this.socket || this.isConnecting) return this.socket;

    try {
      this.isConnecting = true
      this.socket = await WebSocket.connect(this.url);
      this.reconnectAttempts = 0;

      this.socket.addListener(this.handleMessage.bind(this));
      console.log("🎧 WebSocket Connected", this.socket);
    } catch (error) {
      if (this.reconnectAttempts < this.maxReconnectAttempts) {
        this.handleReconnect();
      }
    }

    return this.socket;
  }

  private async handleReconnect(): Promise<void> {
    this.reconnectAttempts++;
    const delay = Math.min(1000 * 2 ** this.reconnectAttempts, 30000);
    
    if (this.reconnectTimer !== null) {
      clearTimeout(this.reconnectTimer);
    }

    this.reconnectTimer = window.setTimeout(async () => {
      await this.connect();
    }, delay);
  }

  private handleMessage(event: Message): void {

    try {
      if (event.type !== "Text") return;

      const parsed = JSON.parse(event.data);
      if (parsed?.type !== this.eventType) return;

      const data: T = JSON.parse(parsed.data);

      this.notifyListeners(data);
    } catch (error) {
      console.error("⚠️ Error handling WebSocket message:", error);
    }
  }

  async subscribe(
    listener: WebSocketEventHandler<T>
  ): Promise<() => void> {

    this.listeners.push(listener);
    return () => {
      this.listeners = this.listeners.filter((l) => l !== listener);

      if (this.listeners.length === 0) {        
        this.disconnect();
      }
    };
  }

  async disconnect(): Promise<void> {
    if (this.socket) {      
      await this.socket.disconnect();
      this.socket = null;
      this.isConnecting = false;
    }

    if (this.reconnectTimer !== null) {
      clearTimeout(this.reconnectTimer);
      this.reconnectTimer = null;
    }

    this.listeners = [];
  }

  private notifyListeners(data: T): void {
    for (const listener of this.listeners) {
      listener(data);
    }
  }

  public isConnected() {
    return this.isConnecting;
  }
  
}
