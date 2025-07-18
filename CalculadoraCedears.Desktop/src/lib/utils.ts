import { Cedears } from "@/types/cedears";
import { clsx, type ClassValue } from "clsx";
import { twMerge } from "tailwind-merge";
import { load } from '@tauri-apps/plugin-store';
import { User } from "@/types/user";
import { ApiResponse } from "@/types/api-response";
import { invoke } from "@tauri-apps/api/core";

const STORE_KEY = "currentUser";

export const toastBaseStyle = {
  success: {
    backgroundColor: "#d1fae5",
    background: "#d1fae5",
    borderColor: "#065f46",
    color: "#065f46",
  },
  error: {
    backgroundColor: "#fee2e2",
    background: "#fee2e2",
    borderColor: "#991b1b",
    color: "#991b1b",
  },
  info: {
    backgroundColor: "#dbeafe",
    background: "#dbeafe",
    borderColor: "#1e3a8a",
    color: "#1e3a8a",
  },
  warning: {
    backgroundColor: "#fef3c7",
    background: "#fef3c7",
    borderColor: "#92400e",
    color: "#92400e",
  },
  default: {},
};

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function formatCurrency(value: number) {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(value);
}

export function formatPercent(value: number) {
  return `(${value.toFixed(2)}%)`;
}

export function getTotalChange(cedear: Cedears) {
  const total = cedear.cedearsStockHoldings.reduce(
    (acc, curr) => {
      acc.sinceChange += curr.sinceChange;
      return acc;
    },
    { sinceChange: 0 }
  );

  return total;
}

export function getTotalChangeSummary(cedear: Cedears) {

  const totalPurchase = parseFloat(
    cedear.cedearsStockHoldings
      .reduce((acc, h) => acc + h.purchaseValueUsd, 0)
      .toFixed(2)
  );
  const totalCurrent = parseFloat(
    cedear.cedearsStockHoldings
      .reduce((acc, h) => acc + h.currentValueUsd, 0)
      .toFixed(2)
  );

  const sinceChange = totalCurrent - totalPurchase;
  const sinceChangePercent =
    totalPurchase > 0 ? (totalCurrent / totalPurchase - 1) * 100 : 0;

  return {
    totalPurchase,
    totalCurrent,
    sinceChange,
    sinceChangePercent,
  };
}

async function getStore() {
  return load('user-store.json', { autoSave: true });
}

export async function deleteStore() {
  const store = await getStore();
  await store.delete(STORE_KEY);
  await store.save();
}

export async function setCurrentUser(user: User) {
    const store = await getStore();
    await store.set(STORE_KEY, user);
    await store.save();
}

export async function getCurrentUser(): Promise<User> {
  try {
    const store = await getStore();
    const user = await store.get<User>(STORE_KEY);
    if (!user) throw new Error("No hay usuario logueado");
    return user;
  } catch (error) {
    console.error('Failed to get stored user:', error);
    throw error;
  }
}

export async function apiRequest<T = any>({
  endpoint,
  method,
  body,
  headers = {},
}: ApiRequestOptions): Promise<T> {
  const currentUser = await getCurrentUser();

  const finalHeaders: Record<string, string> = {
    Authorization: `Bearer ${currentUser.token!.trim()}`,
    "X-Refresh-Token": currentUser.refresh_token!,
    ...headers,
  };

  const response: ApiResponse = await invoke("http_request", {
    endpoint,
    method,
    body: body ? JSON.stringify(body) : null,
    headers: finalHeaders,
  });

  if (response.success) {
    return response.data as T;
  } else {
    const errorMessage =
      typeof response.error === "string"
        ? response.error
        : response.error?.message ?? "Error desconocido";
    throw new Error(errorMessage);
  }
}