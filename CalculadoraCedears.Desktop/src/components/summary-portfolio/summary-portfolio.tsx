import { Button } from "@/components/ui/button";
import { formatCurrency } from "@/lib/utils";

export default function SummaryPortfolio({
  portfolioValue,
  todaysGain,
  todaysGainPercent,
}: SummaryPortfolio) {
  return (
    <div className="flex justify-between items-center mb-8">
      <div className="flex space-x-12">
        <div>
          <h3 className="text-sm text-gray-500">Tu Portfolio</h3>
          <h2 className="text-3xl font-bold">
            {formatCurrency(portfolioValue)}
          </h2>
        </div>
        <div>
          <h3 className="text-sm text-gray-500">Ganancia\Perdida Hoy</h3>
          <h2 className="text-2xl font-medium text-green-500">
            +{formatCurrency(todaysGain)} ({todaysGainPercent}%)
          </h2>
        </div>
        <div>
          <h3 className="text-sm text-gray-500">Dolar CCL</h3>
          <h2 className="text-2xl font-medium text-green-500">
            +{formatCurrency(todaysGain)} ({todaysGainPercent}%)
          </h2>
        </div>
      </div>

      <div className="flex space-x-2">
        <Button variant="outline" size="sm">
          Ticker
        </Button>
      </div>
    </div>
  );
}
