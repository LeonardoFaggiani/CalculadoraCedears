import { CedearsStockResponse } from "@/types/cedears";
import { CreateCedear } from "../types/create-cedear";
import { BrokerResponse } from "../types/broker-response";
import { CedearResponse } from "@/types/cedear-response";
import { UpdateCedear } from "@/types/update-cedear";
import { LogOutUser } from "@/types/logout-user";
import { apiRequest } from "@/lib/utils";
import { DollarCCLQuote } from "@/types/dollarCCL-quote";

export async function getCedearsAsync(): Promise<CedearResponse> {
    return await apiRequest({ endpoint: "Cedears", method: "GET" });
}

export async function getBrokersAsync(): Promise<BrokerResponse> {
    return await apiRequest({ endpoint: "Broker", method: "GET" });
}

export async function getCedearStockHoldingAsync(userId: string): Promise<CedearsStockResponse> {
    return await apiRequest({
        endpoint: `CedearsStockHolding?userId=${userId}`,
        method: "GET",
    });
}

export async function postCedearStockHoldingAsync(createCedearRequest: CreateCedear): Promise<void> {
    return await apiRequest({
        endpoint: "CedearsStockHolding",
        method: "POST",
        body: createCedearRequest,
    });
}

export async function putCedearStockHoldingAsync(updateCedearRequest: UpdateCedear): Promise<void> {
    return await apiRequest({
        endpoint: "CedearsStockHolding",
        method: "PUT",
        body: updateCedearRequest,
    });
}

export async function deleteCedearStockHoldingAsync(cedearStockHoldingId: string): Promise<void> {
    return await apiRequest({
        endpoint: `CedearsStockHolding?cedearsStockHoldingId=${cedearStockHoldingId}`,
        method: "DELETE",
    });
}

export async function postLogOutUserAsync(logOutUserRequest: LogOutUser): Promise<void> {
    return await apiRequest({
        endpoint: "Auth/logout",
        method: "POST",
        body: logOutUserRequest,
    });
}

export async function getDollarCCLQuoteAsync(): Promise<DollarCCLQuote> {
    return await apiRequest({ endpoint: "Cedears/ccl-quote", method: "GET" });
}