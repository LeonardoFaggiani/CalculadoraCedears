import { useContext } from "react";
import { DataContext } from "../context/data-context";

export const useData = () => {
  const ctx = useContext(DataContext);
  if (!ctx) throw new Error("useDataContext must be used within DataProvider");
  return ctx;
};
