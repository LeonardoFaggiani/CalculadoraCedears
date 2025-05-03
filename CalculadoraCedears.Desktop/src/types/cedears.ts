import { StockHoldings } from "./stock-holdings";

  export type CedearsStockResponse = {
    cedearWithStockHoldings: Cedears[];
  };
  
  export type Cedears = {
    id: string;
    name: string;
    ticker: string;
    ratio: number;
    price: number;
    priceChangeDirection:string;
    cedearsStockHoldings: StockHoldings[];
  };  