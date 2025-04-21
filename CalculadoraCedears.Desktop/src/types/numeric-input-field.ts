import { FieldValues, Path, UseFormReturn } from "react-hook-form";

type NumericType = "int" | "float";

export type NumericInputField<T extends FieldValues> = {
  form: UseFormReturn<T>;
  name: Path<T>;
  label: string;
  placeholder?: string;
  numericType?: NumericType;
  required?: boolean;
  prefixSymbol?:string
};
