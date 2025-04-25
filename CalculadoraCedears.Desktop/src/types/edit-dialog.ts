import { StockHoldings } from "./stock-holdings";

export type EditStockHoldingDialog = {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSave: (item: StockHoldings) => void;
  stock: StockHoldings;
};
