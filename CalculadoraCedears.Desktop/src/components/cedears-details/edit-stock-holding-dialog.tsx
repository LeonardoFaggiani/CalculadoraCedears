import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "../ui/dialog";
import { Button } from "../ui/button";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { formSchemaEdit } from "@/lib/form-schema";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { useEffect, useState } from "react";
import { SelectItemField } from "../add-cedears/select-item-field";
import { NumericInputFields } from "../add-cedears/numeric-input-field";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import { CalendarIcon } from "lucide-react";
import { Calendar } from "../ui/calendar";
import { format } from "date-fns";
import { es } from "date-fns/locale";
import { EditStockHoldingDialog } from "@/types/edit-dialog";
import { useData } from "@/hooks/useData";

export function EditDialog({
  open,
  onOpenChange,
  onSave,
  stock,
}: EditStockHoldingDialog) {
  const [openCalendar, setOpenCalendar] = useState(false);
  const { brokers } = useData();

  const form = useForm({
    resolver: zodResolver(formSchemaEdit),
    defaultValues: {
      ...stock,
    },
  });

  useEffect(() => {
    if (stock) {
      form.reset({
        quantity: stock.quantity,
        sinceDate: new Date(stock.sinceDate),
        purchasePriceArs: stock.purchasePriceArs,
        exchangeRateCcl: stock.exchangeRateCcl,
        brokerId: stock.brokerId.toString(),
      });
    }
  }, [form, stock]);

  function onSubmit(values: any) {
    onSave(values);
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Editar cedear</DialogTitle>
        </DialogHeader>
        <DialogDescription>Modifica los datos del cedear.</DialogDescription>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <SelectItemField
              form={form}
              name="brokerId"
              label="Broker"
              items={brokers}
              placeholder="Seleccionar broker..."
            />

            <NumericInputFields
              form={form}
              name="quantity"
              label="Cantidad"
              placeholder="Cantidad"
              numericType="int"
              required
            />

            <FormField
              control={form.control}
              name="sinceDate"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Fecha</FormLabel>
                  <FormControl>
                    <Popover open={openCalendar} onOpenChange={setOpenCalendar}>
                      <PopoverTrigger asChild>
                        <Button
                          variant="outline"
                          className="w-full justify-start text-left font-normal"
                          id="sinceDate"
                        >
                          <CalendarIcon className="mr-2 h-4 w-4" />
                          {field.value
                            ? format(field.value, "PPP", { locale: es })
                            : "Seleccionar fecha de compra"}
                        </Button>
                      </PopoverTrigger>
                      <PopoverContent className="w-auto p-0">
                        <Calendar
                          mode="single"
                          selected={field.value}
                          onSelect={(date) => {
                            field.onChange(date);
                            setOpenCalendar(false);
                          }}
                          initialFocus
                        />
                      </PopoverContent>
                    </Popover>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <NumericInputFields
              form={form}
              name="exchangeRateCcl"
              label="Dolar CCL"
              placeholder="0.00"
              numericType="float"
              required
              prefixSymbol="$"
            />

            <NumericInputFields
              form={form}
              name="purchasePriceArs"
              label="Precio Compra (Ars)"
              placeholder="0.00"
              numericType="float"
              required
              prefixSymbol="$"
            />

            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                className="text-white hover:text-white hover:bg-red-200 bg-red-400 cursor-pointer"
                onClick={() => onOpenChange(false)}
              >
                Cancelar
              </Button>
              <Button
                className="text-white hover:bg-green-300 bg-green-500 cursor-pointer"
                type="submit"
              >
                Guardar cambios
              </Button>
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
