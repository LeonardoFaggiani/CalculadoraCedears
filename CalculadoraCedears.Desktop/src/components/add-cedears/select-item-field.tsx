import { SelectFieldProps } from "@/types/select-field-props";
import { useMemo, useState } from "react";
import { FieldValues } from "react-hook-form";
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from "../ui/command";
import { Check, ChevronsUpDown } from "lucide-react";
import { Button } from "../ui/button";
import { cn } from "@/lib/utils";

export function SelectItemField<T extends FieldValues>({
  form,
  name,
  label,
  items,
  selected,
  setSelected,
  placeholder = "Seleccionar...",
  className = "w-full justify-between",
  popoverWidth = "w-[450px]",
  searchable = false,
  emptyText = "No se encontraron resultados.",
  searchPlaceholder = "Ingresa ticker...",
  customFilter,
}: SelectFieldProps<T>) {
  const [open, setOpen] = useState(false);

  const [searchTerm, setSearchTerm] = useState("");

  const filteredOptions = useMemo(() => {
    if (!searchable || searchTerm.trim() === "") return items;
    return items.filter((o) =>
      customFilter
        ? customFilter(o, searchTerm)
        : o.label.toLowerCase().includes(searchTerm.toLowerCase()) ||
          o.id.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [items, searchTerm, searchable, customFilter]);

  return (
    <FormField
      control={form.control}
      name={name}
      render={() => (
        <FormItem>
          <FormLabel>{label}</FormLabel>
          <FormControl>
            <Popover open={open} onOpenChange={setOpen}>
              <PopoverTrigger asChild>
                <Button variant="outline" role="combobox" className={className}>
                  {selected
                    ? items.find((o) => o.id === selected)?.label
                    : placeholder}
                  <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                </Button>
              </PopoverTrigger>
              <PopoverContent className={`${popoverWidth} p-0`}>
                <Command>
                  {searchable && (
                    <CommandInput
                      placeholder={searchPlaceholder}
                      value={searchTerm}
                      onValueChange={setSearchTerm}
                    />
                  )}
                  {searchable && <CommandEmpty>{emptyText}</CommandEmpty>}
                  <CommandList>
                    <CommandGroup>
                      {filteredOptions.map((o) => (
                        <CommandItem
                          key={o.id}
                          value={o.id}
                          onSelect={(currentValue) => {
                            setSelected(
                              currentValue === selected ? "" : currentValue
                            );
                            setOpen(false);
                            setSearchTerm("");
                          }}
                        >
                          <Check
                            className={cn(
                              "mr-2 h-4 w-4",
                              selected === o.id ? "opacity-100" : "opacity-0"
                            )}
                          />
                          {o.label}
                        </CommandItem>
                      ))}
                    </CommandGroup>
                  </CommandList>
                </Command>
              </PopoverContent>
            </Popover>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
