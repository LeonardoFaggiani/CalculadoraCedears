import { z } from "zod";

export const formSchema = z.object({
  broker: z.string().min(1, "Debe seleccionar un Broker"),
  cedear: z.string().min(1, "Debe seleccionar un Cedear"),
  quantity: z.coerce.number().min(1, "Campo obligatorio y debe ser mayor a 0"),
  sinceDate: z.date({ required_error: "Debe seleccionar una fecha" }),
  exchangeRateCcl: z.coerce.number().min(0.01, "Campo obligatorio y debe ser mayor a 0"),
  purchasePriceArs: z.coerce.number().min(0.01, "Campo obligatorio y debe ser mayor a 0"),
});

export const formSchemaEdit = z.object({
  brokerId: z.string().min(1, "Debe seleccionar un Broker"),  
  quantity: z.coerce.number().min(1, "Campo obligatorio y debe ser mayor a 0"),
  sinceDate: z.date({ required_error: "Debe seleccionar una fecha" }),
  exchangeRateCcl: z.coerce.number().min(0.01, "Campo obligatorio y debe ser mayor a 0"),
  purchasePriceArs: z.coerce.number().min(0.01, "Campo obligatorio y debe ser mayor a 0"),
});