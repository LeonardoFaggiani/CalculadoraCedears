export type StockHoldings = {
  id: string;
  quantity: number;
  sinceDate: Date;
  exchangeRateCcl: number;
  purchasePriceArs: number;
  purchasePriceUsd: number;
  purchaseValueUsd: number;
  currentPriceUsd: number;
  currentValueUsd: number;
  sinceChange: number;
  sinceChangePercent: number;
  cedearId:string;
  brokerId:string;
}