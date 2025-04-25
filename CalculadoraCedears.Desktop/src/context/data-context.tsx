import { getAddCedearData } from "@/loaders/loader-add-cedears";
import { DataContextType } from "@/types/data-context-type";
import { ListItem } from "@/types/list-item";
import React, { createContext, useContext, useState, useEffect } from "react";

const DataContext = createContext<DataContextType | undefined>(undefined);

export const DataProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {

  const [brokers, setBrokers] = useState<ListItem[]>([]);
  const [cedears, setCedears] = useState<ListItem[]>([]);

  const fetchData = async () => {
    const addCedearDataResponse = await getAddCedearData();
    setBrokers(addCedearDataResponse.brokers);
    setCedears(addCedearDataResponse.cedears);
  };

  useEffect(() => {
    fetchData();
  }, []);

  return (
    <DataContext.Provider value={{ brokers, cedears, refreshData: fetchData }}>
      {children}
    </DataContext.Provider>
  );
};

export const useDataContext = () => {
  const ctx = useContext(DataContext);
  if (!ctx) throw new Error("useDataContext must be used within DataProvider");
  return ctx;
};
