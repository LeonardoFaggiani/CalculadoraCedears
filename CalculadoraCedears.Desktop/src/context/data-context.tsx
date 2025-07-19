import { useAuth } from "@/hooks/useAuth";
import { setApiToken } from "@/lib/utils";
import { getAddCedearData } from "@/loaders/loader-add-cedears";
import { DataContextType } from "@/types/data-context-type";
import { DollarCCLQuote } from "@/types/dollarCCL-quote";
import { ListItem } from "@/types/list-item";
import React, { createContext, useState, useCallback } from "react";

export const DataContext = createContext<DataContextType | undefined>(undefined);

export const DataProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {

  const [brokers, setBrokers] = useState<ListItem[]>([]);
  const [cedears, setCedears] = useState<ListItem[]>([]);
  const [dollarCCLQuote, setDollarCCLQuote] = useState<DollarCCLQuote>();
  const { getCurrentUser } = useAuth();

    const fetchData = useCallback(async () => {
      try {
        
        const user = await getCurrentUser();
        setApiToken(user.token!, user.refresh_token!);

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