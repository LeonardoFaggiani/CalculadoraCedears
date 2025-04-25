"use client";

import { useEffect, useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";
import { getCedearStockHoldingAsync } from "@/api/cedears-api";
import { CedearsStockResponse } from "@/types/cedears";

export default function Dashboard() {

  const [cedearsStockResponse, setCedearsStockHolding] = useState<CedearsStockResponse>();
  const [loading, setLoading] = useState<boolean>(true);

  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {}
  );

  useEffect(() => {
    loadCedears();
  }, []);

  const loadCedears = async () => {
    try {
      setLoading(true);
      await getCedearStockHoldingAsync().then(setCedearsStockHolding).catch(console.log);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const toggleCedear = (cedearsId: string) => {
    setExpandedTicker((prev) => ({
      ...prev,
      [cedearsId]: !prev[cedearsId],
    }));
  };

  const portfolioValue = 274980;
  const todaysGain = 706.22;
  const todaysGainPercent = 0.3;

  return (
    <>
      <SummaryPortfolio
        portfolioValue={portfolioValue}
        todaysGain={todaysGain}
        todaysGainPercent={todaysGainPercent}
      />
      <CedearsInfoTabs
        cedears={cedearsStockResponse?.cedearWithStockHoldings}
        expandedTicker={expandedTicker}
        toggleCedear={toggleCedear}
      />
    </>
  );
}
