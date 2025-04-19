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

export default function CedearsDetailTable({
  ticker,
  stockHoldings,
}: {
  ticker:string,
  stockHoldings: StockHoldings[];
}) {
  return (
    <Table>
      <TableHeader>
        <TableRow className="hover:bg-gray-100">
          <TableHead className="w-[120px]">FECHA COMPRA</TableHead>
          <TableHead className="text-center">CCL COMPRA</TableHead>
          <TableHead className="text-center">CANT.</TableHead>
          <TableHead className="text-center">P. COMPRA</TableHead>
          <TableHead className="text-center">PRECIO (USD)</TableHead>
          <TableHead className="text-center">VALOR COMPRA (USD)</TableHead>
          <TableHead className="text-center">P. ACTUAL (USD)</TableHead>
          <TableHead className="text-center">VALOR ACTUAL (USD)</TableHead>
          <TableHead className="text-center">VAR. ($)</TableHead>
          <TableHead className="text-center">VAR. (%)</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {stockHoldings.map((stock) => (          
          <TableRow key={ticker} className="hover:bg-gray-100">
            <TableCell className="font-center">{new Date(stock.sinceDate).toLocaleDateString()}</TableCell>
            <TableCell className="text-center">{stock.exchangeRateCcl}</TableCell>
            <TableCell className="text-center">{stock.quantity}</TableCell>
            <TableCell className="text-center">${stock.purchasePriceArs}</TableCell>
            <TableCell className="text-center">{stock.purchasePriceUsd}</TableCell>
            <TableCell className="text-center">{stock.purchaseValueUsd}</TableCell>
            <TableCell className="text-center">{stock.currentPriceUsd}</TableCell>
            <TableCell className="text-center">{stock.currentValueUsd}</TableCell>
            <TableCell className="text-center"><PriceGainLoss isPercent={false} value={stock.sinceChange} /></TableCell>
            <TableCell className="text-center"><PriceGainLoss isPercent={true} value={stock.sinceChangePercent} /></TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
