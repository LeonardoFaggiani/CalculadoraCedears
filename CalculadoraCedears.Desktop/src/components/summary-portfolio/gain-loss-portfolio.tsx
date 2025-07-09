import { formatCurrency } from "@/lib/utils";
import { DollarSign, TrendingDown, TrendingUp } from "lucide-react";
import { GainLossPortfolioProps } from "@/types/gain-loss-portfolio";
import { Card, CardHeader } from "../ui/card";

export function GainLossPortfolio({
  portfolioValue,
  dolarCCL,
}: GainLossPortfolioProps) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
      <Card className="md:col-span-1">
        <CardHeader className="pb-3">
          <div>
            <p className="text-sm font-medium text-slate-600">Tu Portfolio</p>
            <h2 className="text-3xl font-bold text-slate-900">
              {formatCurrency(portfolioValue)}
            </h2>
          </div>
        </CardHeader>
      </Card>

      <Card>
        <CardHeader className="pb-3">
          <div className="flex items-center justify-between">
            <div>
              <p className="text-sm font-medium text-slate-600">Dólar CCL</p>
              <div
                className={`flex items-center ${
                  dolarCCL.percentage >= 0 ? "text-emerald-600" : "text-red-600"
                }`}
              >
                {dolarCCL.percentage >= 0 ? (
                  <TrendingUp className="w-4 h-4 mr-1" />
                ) : (
                  <TrendingDown className="w-4 h-4 mr-1" />
                )}

                <span className="text-xl font-bold">
                  {dolarCCL.amount >= 0 ? "+" : ""}
                  {formatCurrency(dolarCCL.amount!)} ({dolarCCL.percentage}%)
                </span>
              </div>
            </div>

            <DollarSign
              className={`w-8 h-8 ${
                dolarCCL.percentage >= 0 ? "text-emerald-600" : "text-red-600"
              }`}
            />
          </div>
        </CardHeader>
      </Card>
    </div>
  );
}
