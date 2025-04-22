import { getBrokersAsync, getCedearsAsync } from "@/api/cedears-api";
import { Broker } from "@/types/broker-response";
import { Cedear } from "@/types/cedear-response";
import { ListItem } from "@/types/list-item";

export async function getAddCedearData() {
  const [brokersResponse, cedearsResponse] = await Promise.all([
    getBrokersAsync(),
    getCedearsAsync(),
  ]);

  if (!brokersResponse.brokers || !cedearsResponse.cedears) {
    throw new Response("Error al cargar datos", { status: 500 });
  }

  const brokers: ListItem[] = brokersResponse.brokers.map((x: Broker) => ({
    id: x.id.toString(),
    label: x.name
  }));

  const cedears: ListItem[] = cedearsResponse.cedears.map((x: Cedear) => ({
    id: x.id,
    label: `${x.name} - (${x.ticker})`
  }));

  return { brokers, cedears };
}
