interface CedearsInfoTabs {
    cedears: Cedears[];
    expandedTicker: Record<string, boolean>;
    toggleCedear: (ticker: string) => void;
  }