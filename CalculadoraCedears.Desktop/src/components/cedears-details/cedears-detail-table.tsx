import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

export default function CedearsDetailTable({
  holdings,
}: {
  holdings: StockHolding[];
}) {
  return (
    <Table>
      <TableHeader>
        <TableRow className="hover:bg-gray-100">
          <TableHead className="w-[120px]">TICKER</TableHead>
          <TableHead className="text-right">PRICE</TableHead>
          <TableHead className="text-right">CHANGE</TableHead>
          <TableHead className="text-center">52-WEEK RANGE</TableHead>
          <TableHead className="text-right">SHARES</TableHead>
          <TableHead className="text-right">VALUE</TableHead>
          <TableHead className="text-right">TOTAL GAIN/LOSS</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {holdings.map((stock) => (
          <TableRow key={stock.ticker} className="hover:bg-gray-100">
            <TableCell className="font-medium">
              {stock.ticker}
              <div className="text-xs text-gray-500">{stock.name}</div>
            </TableCell>
            <TableCell className="text-right">${stock.price}</TableCell>
            <TableCell
              className={`text-right ${
                stock.change >= 0 ? "text-green-500" : "text-red-500"
              }`}
            >
              {stock.change >= 0 ? "+" : ""}
              {stock.change} ({stock.changePercent}%)
              {stock.change >= 0 ? "▲" : "▼"}
            </TableCell>
            <TableCell>
              <div className="flex items-center">
                <span className="text-xs mr-1">{stock.weekLow}</span>
                <div className="h-1 flex-1 bg-gray-200 rounded-full">
                  <div
                    className="h-1 bg-gray-500 rounded-full"
                    style={{
                      width: `${
                        ((stock.currentPosition - stock.weekLow) /
                          (stock.weekHigh - stock.weekLow)) *
                        100
                      }%`,
                    }}
                  ></div>
                </div>
                <span className="text-xs ml-1">{stock.weekHigh}</span>
              </div>
            </TableCell>
            <TableCell className="text-right">{stock.shares}</TableCell>
            <TableCell className="text-right">
              ${stock.value.toLocaleString()}
            </TableCell>
            <TableCell
              className={`text-right ${
                stock.totalGainLoss >= 0 ? "text-green-500" : "text-red-500"
              }`}
            >
              {stock.totalGainLoss >= 0 ? "+" : ""}
              {stock.totalGainLoss.toLocaleString()} (
              {stock.totalGainLossPercent}%)
              {stock.totalGainLoss >= 0 ? "▲" : "▼"}
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
