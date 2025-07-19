import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { Toaster } from "./components/ui/sonner";
import { DataProvider } from "./context/data-context";
import { WebSocketProvider } from "./context/web-socket-context";
import { AuthProvider } from "./context/auth-context";

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <AuthProvider>
      <DataProvider>
        <WebSocketProvider>
          <App />
          <Toaster />
        </WebSocketProvider>
      </DataProvider>
    </AuthProvider>
  </React.StrictMode>
);
