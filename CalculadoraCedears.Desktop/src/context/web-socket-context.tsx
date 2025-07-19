  import React, { createContext, useEffect, useState } from "react";
import { CedearsStockResponse } from "@/types/cedears";
import { WebSocketClient } from "@/services/web-socket.client";
import { User } from "@/types/user";
import { useAuth } from "@/hooks/useAuth";

type WebSocketContextType = {
  client: WebSocketClient<CedearsStockResponse> | null;
};

export const WebSocketContext = createContext<WebSocketContextType | undefined>(
  undefined
);

export const WebSocketProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {

  const [client, setClient] = useState<WebSocketClient<CedearsStockResponse> | null>(null);
  const [user, setUser] = useState<User | null>(null);
  const { getCurrentUser } = useAuth();
  
  useEffect(() => {
    const loadUser = async () => {
      const user = await getCurrentUser();
      setUser(user);
    };

    loadUser();
  }, []);

  useEffect(() => {
    let cancelled = false;

    const setup = async () => {
      try {
        if (client) return;

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
  }, [user?.id]);

  return (
    <WebSocketContext.Provider value={{ client }}>
      {children}
    </WebSocketContext.Provider>
  );
};
