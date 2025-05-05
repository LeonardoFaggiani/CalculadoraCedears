import { formatCurrency } from "@/lib/utils";
import { useNavigate } from "react-router-dom";
import { Button } from "../ui/button";
import { BadgePlus } from "lucide-react";
import { GainLossPortfolioProps } from "@/types/gain-loss-portfolio";

export function GainLossPortfolio({
    portfolioValue,
    todayGainLoss,
    dolarCCL,
    hasCedears,
  }: GainLossPortfolioProps) {
    const navigate = useNavigate();
  
    return (
      <div className="flex justify-between items-start mb-6">
        <div className="space-y-4">
          <div>
            <p className="text-sm text-muted-foreground">Tu Portfolio</p>
            <h2 className="text-3xl font-bold">{formatCurrency(portfolioValue)}</h2>
          </div>
  
          <div className="flex gap-6">
            <div>
              <p className="text-sm text-muted-foreground">Ganancia/Pérdida Hoy</p>
              <p
                className={`text-base font-medium ${
                  todayGainLoss.amount >= 0 ? "text-emerald-500" : "text-red-500"
                }`}
              >
                {todayGainLoss.amount >= 0 ? "+" : ""}
                {formatCurrency(todayGainLoss.amount)} ({todayGainLoss.percentage}%)
              </p>
            </div>
  
            <div>
              <p className="text-sm text-muted-foreground">Dolar CCL</p>
              <p
                className={`text-base font-medium ${
                  dolarCCL.amount >= 0 ? "text-emerald-500" : "text-red-500"
                }`}
              >
                {dolarCCL.amount >= 0 ? "+" : ""}
                {formatCurrency(dolarCCL.amount)} ({dolarCCL.percentage}%)
              </p>
            </div>
          </div>
        </div>
  
        {hasCedears ? (
          <Button
            className="bg-emerald-500 hover:bg-emerald-600 cursor-pointer"
            onClick={() => navigate("/cedear")}
          >
            <BadgePlus />
            Agregar CEDEAR
          </Button>
        ) : (
          <span></span>
        )}
      </div>
    );
  }