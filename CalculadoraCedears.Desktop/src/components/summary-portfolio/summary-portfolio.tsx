import { SummaryPortfolioProps } from "@/types/summary-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";
import { useState } from "react";
import { LoaderSkeleton } from "../loader-skeleton/loader-skeleton";
import EmptyStatePortfolio from "./empty-state-portfolio";
import { GainLossPortfolio } from "./gain-loss-portfolio";

export default function SummaryPortfolio({
  portfolioValue,
  cedears = [],
  dolarCCL,
  onRefresh,
  loading = false,
}: SummaryPortfolioProps & { loading?: boolean }) {
  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {}
  );

  const toggleCedear = (cedearsId: string) => {
    setExpandedTicker((prev) => ({
      ...prev,
      [cedearsId]: !prev[cedearsId],
    }));
  };

  return (
    <div className="w-full">
      {loading ? (
        <>
          <LoaderSkeleton rows={5} />
        </>
      ) : !loading && cedears.length === 0 ? (
        <>
          <GainLossPortfolio
            portfolioValue={portfolioValue}            
            dolarCCL={dolarCCL}
            hasCedears={cedears.length > 0}
          />
          <EmptyStatePortfolio />
        </>
      ) : (
        <div className="grid gap-4">
          <>
            <GainLossPortfolio
              portfolioValue={portfolioValue}              
              dolarCCL={dolarCCL}
              hasCedears={cedears.length > 0}
            />
            <CedearsInfoTabs
              cedears={cedears}
              expandedTicker={expandedTicker}
              toggleCedear={toggleCedear}
              onRefresh={onRefresh}
            />
          </>
        </div>
      )}
    </div>
  );
}
