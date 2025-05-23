import { FieldValues, Path, UseFormReturn } from "react-hook-form";
import { ListItem } from "./list-item";

export type SelectFieldProps<T extends FieldValues> = {
  form: UseFormReturn<T>
  name: Path<T>;
  label?: string;
  items: ListItem[];
  placeholder?: string
  className?: string
  popoverWidth?: string
  searchPlaceholder?:string
  searchable?: boolean
  emptyText?: string
};