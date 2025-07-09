import { getBrokersAsync, getDollarCCLQuoteAsync, getCedearsAsync } from "@/api/cedears-api";
import { Broker } from "@/types/broker-response";
import { Cedears } from "@/types/cedears";
import { ListItem } from "@/types/list-item";

export async function getAddCedearData() {
  const [brokersResponse, cedearsResponse, dollarCCLQuote] = await Promise.all([
    getBrokersAsync(),
    getCedearsAsync(),
    getDollarCCLQuoteAsync(),
  ]);

  if (!brokersResponse.brokers || !cedearsResponse.cedears) {
    throw new Response("Error al cargar datos", { status: 500 });
  }

  const brokers: ListItem[] = brokersResponse.brokers.map((x: Broker) => ({
    id: x.id.toString(),
    label: x.name
  }));

  const cedears: ListItem[] = cedearsResponse.cedears.map((x: Cedears) => ({
    id: x.id,
    label: `${x.name} - (${x.ticker})`
  }));

  return { brokers, cedears, dollarCCLQuote };
}
