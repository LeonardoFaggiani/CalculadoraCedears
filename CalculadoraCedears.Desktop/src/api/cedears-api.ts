import { CedearsStockResponse } from "@/types/cedears";
import { CreateCedear } from "../types/create-cedear";
import { BrokerResponse } from "../types/broker-response";
import { CedearResponse } from "@/types/cedear-response";
import { UpdateCedear } from "@/types/update-cedear";
import { invoke } from "@tauri-apps/api/core";
import { getCurrentUser } from "@/services/auth.service";

export async function getCedearsAsync()
: Promise<CedearResponse> {
  try {
    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token!.trim()}`,
    };

    const response = await invoke<any>("http_request", {
      endpoint: `Cedears`,
      method: "GET",
      body: null,
      headers: headers,
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || "Error obteniendo Cedears");
    }
  } catch (error) {
    console.error("Error en getCedearsAsync:", error);
    throw error;
  }
}

export async function getBrokersAsync()
: Promise<BrokerResponse> {
  try {
    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token!.trim()}`,
    };

    const response = await invoke<any>("http_request", {
      endpoint: `Broker`,
      method: "GET",
      body: null,
      headers: headers,
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || "Error obteniendo Brokers");
    }
  } catch (error) {
    console.error("Error en getBrokersAsync:", error);
    throw error;
  }
}

export async function getCedearStockHoldingAsync(userId: string)
: Promise<CedearsStockResponse> {
  try {
    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token!.trim()}`,
    };

    const response = await invoke<any>("http_request", {
      endpoint: `CedearsStockHolding?userId=${userId}`,
      method: "GET",
      body: null,
      headers: headers,
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || "Error obteniendo CedearsStockHolding");
    }
  } catch (error) {
    console.error("Error en getCedearStockHoldingAsync:", error);
    throw error;
  }
}

export async function postCedearStockHoldingAsync(createCedearRequest: CreateCedear)
: Promise<void>  {
  try {

    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token!.trim()}`,
    };

    const response = await invoke<any>('http_request', {
      endpoint: "CedearsStockHolding",
      method: "POST",
      body: JSON.stringify(createCedearRequest),
      headers: headers
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || 'Error creando de CedearStockHolding');
    }
  } catch (error) {
    console.error('Error en postCedearStockHoldingAsync:', error);
    throw error;
  }
}

export async function putCedearStockHoldingAsync(updateCedearRequest: UpdateCedear)
: Promise<void> {
  try {
    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token}`,
    };

    const response = await invoke<any>("http_request", {
      endpoint: "CedearsStockHolding",
      method: "PUT",
      body: JSON.stringify(updateCedearRequest),
      headers: headers,
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || "Error actualizando Cedear");
    }
  } catch (error) {
    console.error("Error en putCedearStockHoldingAsync:", error);
    throw error;
  }
}

export async function deleteCedearStockHoldingAsync(cedearStockHoldingId: string)
: Promise<void>  {
  try {

    const currentUser = await getCurrentUser();

    const headers = {
      Authorization: `Bearer ${currentUser.token}`,
    };

    const response = await invoke<any>('http_request', {
      endpoint: `CedearsStockHolding?cedearsStockHoldingId=${cedearStockHoldingId}`,
      method: "DELETE",
      body: null,
      headers: headers
    });

    if (response.success) {
      return response.data;
    } else {
      throw new Error(response.error || 'Error borrando de CedearStockHolding');
    }
  } catch (error) {
    console.error('Error en deleteCedearStockHoldingAsync:', error);
    throw error;
  }
}