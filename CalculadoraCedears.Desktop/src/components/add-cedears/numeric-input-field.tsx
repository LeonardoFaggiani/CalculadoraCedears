import { NumericInputField } from "@/types/numeric-input-field";
import { ChangeEvent, useState } from "react";
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

  const [inputValue, setInputValue] = useState("");
  
  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value;
  
    // Permitir solo hasta 2 decimales (pero sin parsear aún)
    const decimalRegex = /^\d*\.?\d{0,2}$/;
    const intRegex = /^\d*$/;
  
    const isValid = numericType === "int" ? intRegex.test(val) : decimalRegex.test(val);

    if (isValid) {
      setInputValue(val); // mantener como string mientras el usuario escribe
  
      // Si se puede parsear, lo mandamos al form
      const parsed = numericType === "int" ? parseInt(val, 10) : parseFloat(val);
      if (!isNaN(parsed)) {
        form.setValue(name, parsed as any);
      }
    }
  };  
  
  return (
    <FormField
      control={form.control}
      name={name}
      render={({field}) => (
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
                value={inputValue || field.value}
                placeholder={placeholder}
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