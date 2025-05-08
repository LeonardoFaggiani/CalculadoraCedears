import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { StockHoldings } from "@/types/stock-holdings";
import PriceGainLoss from "../price-gain-loss/price-gain-loss";
import { Button } from "../ui/button";
import { Pencil, Trash2 } from "lucide-react";
import { useState } from "react";

import { DeleteDialog } from "./delete-dialog";
import { EditDialog } from "./edit-stock-holding-dialog";
import { UpdateCedear } from "@/types/update-cedear";
import {
  deleteCedearStockHoldingAsync,
  putCedearStockHoldingAsync,
} from "@/api/cedears-api";
import { ToastService } from "@/services/toast.service";
import { Cedears } from "@/types/cedears";

export default function CedearsDetailTable({
  cedear,
  onRefresh,
}: {
  cedear: Cedears;
  onRefresh: () => Promise<void>;
}) {
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [editDialogOpen, setEditDialogOpen] = useState(false);
  const [selectedStockHoldings, setSelectedStockHoldings] =
    useState<StockHoldings | null>(null);

  const handleDelete = async (id: string) => {
    await ToastService.promise(deleteCedearStockHoldingAsync(id), {
      loading: "Guardando datos...",
      success: "Se ha eliminado correctamente.",
      error: "Hubo un error al borrar los datos.",
    }).finally(() => {
      onRefresh();
      setDeleteDialogOpen(false);
    });
  };

  const handleEdit = async (stockHoldings: StockHoldings) => {
    const request: UpdateCedear = {
      brokerId: stockHoldings.brokerId.toString(),
      exchangeRateCcl: stockHoldings.exchangeRateCcl,
      purchasePriceArs: stockHoldings.purchasePriceArs,
      quantity: stockHoldings.quantity,
      sinceDate: stockHoldings.sinceDate,
      id: selectedStockHoldings!.id,
    };

    await ToastService.promise(putCedearStockHoldingAsync(request), {
      loading: "Guardando datos...",
      success: "Se ha actualizado correctamente.",
      error: "Hubo un error al actualizar los datos.",
    }).finally(() => {
      onRefresh();
      setEditDialogOpen(false);
    });
  };

  return (
    <>
      <Table>
        <TableHeader>
          <TableRow className="hover:bg-gray-100">
            <TableHead className="w-[120px]">FECHA COMPRA</TableHead>
            <TableHead className="text-center">CCL COMPRA ($)</TableHead>
            <TableHead className="text-center">CANT.</TableHead>
            <TableHead className="text-center">P. COMPRA ($)</TableHead>
            <TableHead className="text-center">PRECIO (U$S)</TableHead>
            <TableHead className="text-center">VALOR COMPRA (U$S)</TableHead>
            <TableHead className="text-center">P. ACTUAL (U$S)</TableHead>
            <TableHead className="text-center">VALOR ACTUAL (U$S)</TableHead>
            <TableHead className="text-center">VAR. (U$S)</TableHead>
            <TableHead className="text-center">VAR. (%)</TableHead>
            <TableHead className="text-center"> </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {cedear.cedearsStockHoldings.map((stock) => (
            <TableRow key={stock.id} className="hover:bg-gray-100">
              <TableCell className="font-center">
                {new Date(stock.sinceDate).toLocaleDateString()}
              </TableCell>
              <TableCell className="text-center">
                {stock.exchangeRateCcl}
              </TableCell>
              <TableCell className="text-center">{stock.quantity}</TableCell>
              <TableCell className="text-center">
                {stock.purchasePriceArs}
              </TableCell>
              <TableCell className="text-center">
                {stock.purchasePriceUsd}
              </TableCell>
              <TableCell className="text-center">
                {stock.purchaseValueUsd}
              </TableCell>
              <TableCell className={`text-center transition duration-500 ${cedear.priceChangeDirection === "up" ? "text-green-100"  : cedear.priceChangeDirection === "down" ? "text-red-100" : ""}`}>
                {cedear.price.toFixed(2)}
              </TableCell>
              <TableCell className={`text-center transition duration-500 ${cedear.priceChangeDirection === "up" ? "text-green-100"  : cedear.priceChangeDirection === "down" ? "text-red-100" : ""}`}>
                {stock.currentValueUsd.toFixed(2)}
              </TableCell>
              <TableCell className={`text-center transition duration-500 ${cedear.priceChangeDirection === "up" ? "text-green-100"  : cedear.priceChangeDirection === "down" ? "text-red-100" : ""}`}>
                <PriceGainLoss isPercent={false} value={stock.sinceChange} />
              </TableCell>
              <TableCell className={`text-center transition duration-500 ${cedear.priceChangeDirection === "up" ? "text-green-100"  : cedear.priceChangeDirection === "down" ? "text-red-100" : ""}`}>
                <PriceGainLoss
                  isPercent={true}
                  value={stock.sinceChangePercent}
                />
              </TableCell>
              <TableCell className="text-right">
                <div className="flex justify-end gap-2">
                  <Button
                    variant="outline"
                    className="cursor-pointer"
                    size="icon"
                    onClick={() => {
                      setSelectedStockHoldings(stock);
                      setEditDialogOpen(true);
                    }}
                  >
                    <Pencil className="h-4 w-4" />
                    <span className="sr-only">Editar</span>
                  </Button>
                  <Button
                    variant="outline"
                    size="icon"
                    className="text-red-500 hover:text-red-600 cursor-pointer"
                    onClick={() => {
                      setSelectedStockHoldings(stock);
                      setDeleteDialogOpen(true);
                    }}
                  >
                    <Trash2 className="h-4 w-4" />
                    <span className="sr-only">Eliminar</span>
                  </Button>
                </div>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>

      {selectedStockHoldings && (
        <>
          <DeleteDialog
            open={deleteDialogOpen}
            onOpenChange={setDeleteDialogOpen}
            onConfirm={() => handleDelete(selectedStockHoldings.id)}
          />
          <EditDialog
            open={editDialogOpen}
            onOpenChange={setEditDialogOpen}
            onSave={handleEdit}
            stock={selectedStockHoldings}
          />
        </>
      )}
    </>
  );
}
