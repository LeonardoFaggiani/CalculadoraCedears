import React, { createContext, useEffect, useState } from "react";
import { CedearsStockResponse } from "@/types/cedears";
import { WebSocketClient } from "@/services/web-socket.client";
import { useAuth } from "@/hooks/useAuth";
const wsBaseUrl = import.meta.env.VITE_WS_BASE_URL;

type WebSocketContextType = {
  client: WebSocketClient<CedearsStockResponse> | null;
};

export const WebSocketContext = createContext<WebSocketContextType | undefined>(
  undefined
);

export const WebSocketProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {

  const [client, setClient] = useState<WebSocketClient<CedearsStockResponse> | null>(null);  
  const { user } = useAuth();

  useEffect(() => {
    let cancelled = false;

    const setup = async () => {
      try {
        
        if (client) return;

        if (!user?.id) return;

        const newClient = new WebSocketClient<CedearsStockResponse>(
          `${wsBaseUrl}/ws/stocks?userId=${user.id}`,
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
      setClient(null);
    };
  }, [user?.id]);

  return (
    <WebSocketContext.Provider value={{ client }}>
      {children}
    </WebSocketContext.Provider>
  );
};
