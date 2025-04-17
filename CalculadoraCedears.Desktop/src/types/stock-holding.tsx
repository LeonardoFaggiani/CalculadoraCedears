interface StockHolding {
    ticker: string;
    name: string;
    price: number;
    change: number;
    changePercent: number;
    weekLow: number;
    weekHigh: number;
    currentPosition: number;
    shares: number;
    value: number;
    totalGainLoss: number;
    totalGainLossPercent: number;
  }