interface Cedears {
    id: string;
    name: string;
    accountNumber: string;
    syncStatus: "SYNCED" | "SYNC FAILED" | "MANUAL";
    holdings: StockHolding[];
    value?: number;
    todayChange?: number;
    todayChangePercent?: number;
    sinceChange?: number;
    sinceChangePercent?: number;
  }
  