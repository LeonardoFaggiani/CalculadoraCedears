import { getAddCedearData } from "@/loaders/loader-add-cedears";
import { DataContextType } from "@/types/data-context-type";
import { DollarCCLQuote } from "@/types/dollarCCL-quote";
import { ListItem } from "@/types/list-item";
import React, { createContext, useContext, useState, useCallback } from "react";

const DataContext = createContext<DataContextType | undefined>(undefined);

export const DataProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {

  const [brokers, setBrokers] = useState<ListItem[]>([]);
  const [cedears, setCedears] = useState<ListItem[]>([]);
  const [dollarCCLQuote, setDollarCCLQuote] = useState<DollarCCLQuote>();

    const fetchData = useCallback(async () => {
      try {
        const addCedearDataResponse = await getAddCedearData();
        setBrokers(addCedearDataResponse.brokers);
        setCedears(addCedearDataResponse.cedears);
        setDollarCCLQuote(addCedearDataResponse.dollarCCLQuote);
      } catch (error) {
        console.error("Error al cargar datos:", error);
      }
    }, []);

  return (
    <DataContext.Provider value={{ brokers, cedears, dollarCCLQuote, refreshData: fetchData }}>
      {children}
    </DataContext.Provider>
  );
};

export const useDataContext = () => {
  const ctx = useContext(DataContext);
  if (!ctx) throw new Error("useDataContext must be used within DataProvider");
  return ctx;
};
