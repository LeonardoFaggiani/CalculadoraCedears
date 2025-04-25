"use client";

import { useState } from "react";
import { Check, CalendarIcon, ArrowLeftCircle } from "lucide-react";
import { Button } from "../ui/button";
import { zodResolver } from "@hookform/resolvers/zod";
import { Calendar } from "../ui/calendar";
import { format } from "date-fns";
import { es } from "date-fns/locale";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "../ui/form";
import { useForm } from "react-hook-form";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

import { useNavigate } from "react-router-dom";
import { formSchema } from "@/lib/form-schema";
import { SelectItemField } from "./select-item-field";
import { NumericInputFields } from "./numeric-input-field";
import { postCedearStockHoldingAsync } from "@/api/cedears-api";
import { CreateCedear } from "@/types/create-cedear";
import { useDataContext } from "@/context/data-context";

export default function AddCedear() {
  const [openCalendar, setOpenCalendar] = useState(false);
  const navigate = useNavigate();
  const { brokers, cedears } = useDataContext();

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      broker: "",
      cedear: "",
      quantity: 0,
      sinceDate: undefined,
      exchangeRateCCL: 0,
      purchasePriceArs: 0,
    },
  });

  const onSubmit = async (data: any) => {
    const formValues = form.getValues();

    const request: CreateCedear = {
      brokerId: formValues.broker,
      cedearId: formValues.cedear,
      exchangeRateCCL: formValues.exchangeRateCCL,
      purchasePriceArs: formValues.purchasePriceArs,
      quantity: formValues.quantity,
      sinceDate: formValues.sinceDate,
    };

    await postCedearStockHoldingAsync(request)
      .then(() => {
        navigate("/");
      })
      .catch(console.log);
  };

  return (
    <Card className="w-full max-w-lg mx-auto">
      <CardHeader>
        <CardTitle>Alta de Cedear</CardTitle>
        <CardDescription>
          Ingrese los detalles para agregar un nuevo cedear a su portfolio.
        </CardDescription>
      </CardHeader>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)}>
          <CardContent className="space-y-4">
            <SelectItemField
              form={form}
              name="broker"
              label="Broker"
              items={brokers}
              placeholder="Seleccionar broker..."
            />

            <SelectItemField
              form={form}
              name="cedear"
              label="Cedear"
              items={cedears}
              placeholder="Seleccionar cedear..."
              searchPlaceholder="Ingresar ticker..."
              searchable
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
              name="exchangeRateCCL"
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
          </CardContent>
          <CardFooter className="flex justify-end mt-5">
            <Button
              type="button"
              className="text-white hover:text-white hover:bg-blue-300 bg-blue-500 cursor-pointer mr-5"
              onClick={() => navigate("/")}
            >
              <ArrowLeftCircle /> Volver
            </Button>
            <Button type="submit" className="text-white hover:bg-green-300 bg-green-500 cursor-pointer">
              <Check /> Agregar a Portfolio
            </Button>
          </CardFooter>
        </form>
      </Form>
    </Card>
  );
}
