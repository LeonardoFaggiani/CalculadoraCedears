import { CedearsStockResponse } from "@/types/cedears";
import { fetch } from "@tauri-apps/plugin-http";

export async function getAllCedears(): Promise<CedearsStockResponse> {
  try {
    const response = await fetch(
      "http://localhost:5124/api/CedearsStockHolding",
      {
        method: "GET",
        danger: {
          acceptInvalidCerts: true,
          acceptInvalidHostnames: true,
        },
      }
    );

    if (response.ok) {
      return response.json();
    }
    
    throw new Error(
      `Error ${response.status}: ${JSON.stringify(response.json())}`
    );
  } catch (error) {
    throw error;
  }
}
