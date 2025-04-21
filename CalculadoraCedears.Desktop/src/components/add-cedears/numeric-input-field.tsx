import { NumericInputField } from "@/types/numeric-input-field";
import { ChangeEvent } from "react";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { Input } from "../ui/input";
import { FieldValues } from "react-hook-form";
import clsx from "clsx";

export function NumericInputFields<T extends FieldValues>({
  form,
  name,
  label,
  placeholder = "",
  numericType = "int",
  required = false,
  prefixSymbol = undefined,
}: NumericInputField<T>) {
  const value = form.watch(name) as number | undefined;

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value;

    const isValid =
      numericType === "int" ? /^\d*$/.test(val) : /^\d*\.?\d*$/.test(val);

    if (isValid) {
      const parsed =
        numericType === "int" ? parseInt(val, 10) : parseFloat(val);
      form.setValue(name, isNaN(parsed) ? undefined : (parsed as any));
    }
  };

  return (
    <FormField
      control={form.control}
      name={name}
      render={() => (
        <FormItem>
          <FormLabel>{label}</FormLabel>
          <FormControl>
            <div className="relative">
              {prefixSymbol && (
                <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-gray-500">
                  $
                </span>
              )}
              <Input
                placeholder={placeholder}
                value={value ?? ""}
                onChange={handleChange}
                required={required}
                inputMode="decimal"
                className={clsx(prefixSymbol && "pl-6")}
              />
            </div>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}