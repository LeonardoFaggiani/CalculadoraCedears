import { z } from "zod";

export const formSchema = z.object({
  broker: z.string().min(1, "Debe seleccionar un broker"),
  cedear: z.string().min(1, "Debe seleccionar un CEDEAR"),
  cantidad: z.coerce.number().min(1, "Debe ser mayor a 0"),
  fechaCompra: z.date({ required_error: "Debe seleccionar una fecha" }),
  dolarCcl: z.coerce.number().min(0.01, "Debe ser mayor a 0"),
  precioCompra: z.coerce.number(),
});