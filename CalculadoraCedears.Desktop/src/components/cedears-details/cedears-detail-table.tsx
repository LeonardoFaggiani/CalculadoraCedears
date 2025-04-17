import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

export default function CedearsDetailTable({
  details,
}: {
  details: CedearsDetails[];
}) {
  return (
    <Table>
      <TableHeader>
        <TableRow className="hover:bg-gray-100">
          <TableHead className="w-[120px]">FECHA COMPRA</TableHead>
          <TableHead className="text-center">CCL COMPRA</TableHead>
          <TableHead className="text-center">CANTIDAD</TableHead>
          <TableHead className="text-center">PRECIO COMPRA</TableHead>
          <TableHead className="text-center">PRECIO (USD)</TableHead>
          <TableHead className="text-center">VALOR COMPRA (USD)</TableHead>
          <TableHead className="text-center">PRECIO ACTUAL (USD)</TableHead>
          <TableHead className="text-center">VALOR ACTUAL (USD)</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {details.map((stock) => (
          <TableRow key={stock.ticker} className="hover:bg-gray-100">
            <TableCell className="font-center">
              {stock.sinceDate.toLocaleDateString()}
            </TableCell>
            <TableCell className="text-center">${stock.cclPurchase}</TableCell>
            <TableCell className="text-center">{stock.quantity}</TableCell>
            <TableCell className="text-center">{stock.pricePurchase}</TableCell>
            <TableCell className="text-center">${stock.pricePurchaseUsd}</TableCell>
            <TableCell className="text-center">{stock.valuePurchaseUsd}</TableCell>
            <TableCell className="text-center">{stock.currentPriceUsd}</TableCell>
            <TableCell className="text-center">{stock.valuePurchaseUsd}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
