import React, {
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import { CedearsStockResponse } from "@/types/cedears";
import { WebSocketClient } from "@/services/web-socket.client";
import { getCurrentUser } from "@/lib/utils";

type WebSocketContextType = {
  client: WebSocketClient<CedearsStockResponse> | null;
};

const WebSocketContext = createContext<WebSocketContextType | undefined>(
  undefined
);

export const useWebSocket = () => {
  const context = useContext(WebSocketContext);
  if (!context)
    throw new Error("useWebSocket must be used inside WebSocketProvider");
  return context;
};

export const WebSocketProvider = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  const [client, setClient] = useState<WebSocketClient<CedearsStockResponse> | null>(null);

  useEffect(() => {
    let cancelled = false;

    const setup = async () => {
      try {
        if (client) return;

        const user = await getCurrentUser();
        if (!user?.id) return;

        const newClient = new WebSocketClient<CedearsStockResponse>(
          `ws://localhost:5124/ws/stocks?userId=${user.id}`,
          "cedears_stockholding_updated"
        );

        await newClient.connect();
        if (!cancelled) {
          setClient(newClient);
        }
      } catch (error) {
        console.error("❌ Error initializing WebSocket:", error);
      }
    };

    setup();

    return () => {
      cancelled = true;
      client?.disconnect();
    };
  }, [client]);

  return (
    <WebSocketContext.Provider value={{ client }}>
      {children}
    </WebSocketContext.Provider>
  );
};
