import { Cedears } from "@/types/cedears";
import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function  formatCurrency(value: number) {
  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(value);
};

export function formatPercent(value: number) {
  return `(${value.toFixed(2)}%)`;
};

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
  const totalPurchase = cedear.cedearsStockHoldings.reduce((acc, h) => acc + h.purchaseValueUsd, 0);
  const totalCurrent = cedear.cedearsStockHoldings.reduce((acc, h) => acc + h.currentValueUsd, 0);

  const sinceChange = totalCurrent - totalPurchase;
  const sinceChangePercent = totalPurchase > 0
    ? (totalCurrent / totalPurchase - 1) * 100
    : 0;

  return {
    totalPurchase,
    totalCurrent,
    sinceChange,
    sinceChangePercent,
  };
}

export function getTotalChangePercentage(cedear: Cedears) {  
  const total = cedear.cedearsStockHoldings.reduce(
    (acc, curr) => {
      acc.sinceChangePercent += curr.sinceChangePercent;
      return acc;
    },
    { sinceChangePercent: 0 }
  );

  return total;
}