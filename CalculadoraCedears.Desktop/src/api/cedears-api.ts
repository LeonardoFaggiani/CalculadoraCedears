import { CedearsStockResponse } from "@/types/cedears";
import { fetch } from "@tauri-apps/plugin-http";
import { CreateCedear } from "../types/create-cedear";
import { BrokerResponse } from "../types/broker-response";
import { CedearResponse } from "@/types/cedear-response";

export async function getCedearsAsync(): Promise<CedearResponse> {
  try {
    const response = await fetch("http://localhost:5124/api/Cedears", {
      method: "GET",
      danger: {
        acceptInvalidCerts: true,
        acceptInvalidHostnames: true,
      },
    });

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

export async function getBrokersAsync(): Promise<BrokerResponse> {
  try {
    const response = await fetch("http://localhost:5124/api/Broker", {
      method: "GET",
      danger: {
        acceptInvalidCerts: true,
        acceptInvalidHostnames: true,
      },
    });

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

export async function getCedearStocksAsync(): Promise<CedearsStockResponse> {
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

export async function postCedearAsync(
  createCedearRequest: CreateCedear
): Promise<void> {
  try {
    const response = await fetch(
      "http://localhost:5124/api/CedearsStockHolding",
      {
        method: "POST",
        headers:{
          "Content-Type": "application/json"
        },
        body: JSON.stringify(createCedearRequest),
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
