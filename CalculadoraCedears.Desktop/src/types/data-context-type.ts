import { ListItem } from "./list-item";

export type DataContextType = {
  brokers: ListItem[];
  cedears: ListItem[];
  refreshData: () => Promise<void>;
}