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
import { ListItem } from "@/types/list-item";
import { NumericInputFields } from "./numeric-input-field";

const brokers: ListItem[] = [
  { id: "iol", label: "InvertirOnline" },
  { id: "ppi", label: "Portfolio Personal Inversiones" },
  { id: "balanz", label: "Balanz" },
  { id: "bull", label: "Bull Market Brokers" },
  { id: "cocos", label: "Cocos Capital" },
];

const cedears: ListItem[] = [
  { id: "AAPL", label: "Apple Inc." },
  { id: "AMZN", label: "Amazon.com Inc." },
  { id: "GOOGL", label: "Alphabet Inc. (Google)" },
  { id: "MSFT", label: "Microsoft Corporation" },
  { id: "TSLA", label: "Tesla Inc." },
  { id: "META", label: "Meta Platforms Inc." },
  { id: "NFLX", label: "Netflix Inc." },
  { id: "DIS", label: "The Walt Disney Company" },
  { id: "KO", label: "The Coca-Cola Company" },
  { id: "PEP", label: "PepsiCo Inc." },
  { id: "MELI", label: "MercadoLibre Inc." },
  { id: "NVDA", label: "NVIDIA Corporation" },
  { id: "AMD", label: "Advanced Micro Devices Inc." },
  { id: "INTC", label: "Intel Corporation" },
  { id: "PYPL", label: "PayPal Holdings Inc." },
  { id: "V", label: "Visa Inc." },
  { id: "MA", label: "Mastercard Incorporated" },
  { id: "JPM", label: "JPMorgan Chase & Co." },
  { id: "BAC", label: "Bank of America Corporation" },
  { id: "WMT", label: "Walmart Inc." },
];

export default function AddCedear() {
  const [broker, setBroker] = useState("");
  const [cedear, setCedear] = useState("");
  const [fechaCompra, setFechaCompra] = useState<Date | undefined>(new Date());
  const [openCalendar, setOpenCalendar] = useState(false);
  const navigate = useNavigate();

  const form = useForm({
    resolver: zodResolver(formSchema),
    defaultValues: {
      broker: "",
      cedear: "",
      cantidad: 0,
      fechaCompra: undefined,
      dolarCcl: 0,
      precioCompra: 0,
    },
  });

  const onSubmit = (data: any) => {};

  return (
    <Card className="w-full max-w-lg mx-auto">
      <CardHeader>
        <CardTitle>Alta de Cedear</CardTitle>
        <CardDescription>
          Ingrese los detalles para agregar un nuevo cedear a su portfolio.
        </CardDescription>
      </CardHeader>
      <Form {...form}>
        <form onSubmit={onSubmit}>
          <CardContent className="space-y-4">
            <SelectItemField
              form={form}
              name="broker"
              label="Broker"
              items={brokers}
              selected={broker}
              setSelected={setBroker}
              placeholder="Seleccionar broker..."
            />

            <SelectItemField
              form={form}
              name="cedear"
              label="Cedear"
              items={cedears}
              selected={cedear}
              setSelected={setCedear}
              placeholder="Seleccionar cedear..."
              searchPlaceholder="Ingresar ticker..."
              searchable
            />

            <NumericInputFields
              form={form}
              name="cantidad"
              label="Cantidad"
              placeholder="Cantidad"
              numericType="int"
              required
            />

            <FormField
              control={form.control}
              name="fechaCompra"
              render={() => (
                <FormItem>
                  <FormLabel>Fecha</FormLabel>
                  <FormControl>
                    <Popover open={openCalendar} onOpenChange={setOpenCalendar}>
                      <PopoverTrigger asChild>
                        <Button
                          variant="outline"
                          className="w-full justify-start text-left font-normal"
                          id="fechaCompra"
                        >
                          <CalendarIcon className="mr-2 h-4 w-4" />
                          {fechaCompra
                            ? format(fechaCompra, "PPP", { locale: es })
                            : "Seleccionar fecha"}
                        </Button>
                      </PopoverTrigger>
                      <PopoverContent className="w-auto p-0">
                        <Calendar
                          mode="single"
                          selected={fechaCompra}
                          onSelect={(date) => {
                            setFechaCompra(date);
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
              name="dolarCcl"
              label="Dolar CCL"
              placeholder="0.00"
              numericType="float"
              required
              prefixSymbol="$"
            />

            <NumericInputFields
              form={form}
              name="precioCompra"
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
              className="cursor-pointer mr-5"
              onClick={() => navigate("/")}
            >
              {" "}
              <ArrowLeftCircle /> Volver
            </Button>
            <Button type="submit" className="cursor-pointer">
              {" "}
              <Check /> Agregar a Portfolio
            </Button>
          </CardFooter>
        </form>
      </Form>
    </Card>
  );
}
