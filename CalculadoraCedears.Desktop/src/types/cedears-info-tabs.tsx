import { Cedears } from "./cedears";

export type CedearsInfoTab = {
  cedears: Cedears[] | undefined;
  expandedTicker: Record<string, boolean>;
  toggleCedear: (ticker: string) => void;
};
