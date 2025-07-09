import { DollarCCLQuote } from "./dollarCCL-quote";
import { ListItem } from "./list-item";

export type DataContextType = {
  brokers: ListItem[];
  cedears: ListItem[];
  dollarCCLQuote: DollarCCLQuote | undefined;
  refreshData: () => Promise<void>;
}