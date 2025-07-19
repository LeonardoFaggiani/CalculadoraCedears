import { WebSocketContext } from "@/context/web-socket-context";
import { useContext } from "react";

export const useWebSocket = () => {
  const context = useContext(WebSocketContext);
  if (!context)
    throw new Error("useWebSocket must be used inside WebSocketProvider");
  return context;
};
