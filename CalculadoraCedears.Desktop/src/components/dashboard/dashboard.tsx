"use client";

import { useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";

const cedears: Cedears[] = [
  {
    ticker: "JMIA",
    name: "JUMIA TECHNOLOGIES AG",
    quantity: 325,
    ratio: 1,
    value: 9673,
    todayChange: -61.26,
    todayChangePercent: -0.6,
    sinceChange: -331.94,
    sinceChangePercent: -3.3,
    details: [
      {
        ticker: "JMIA",
        sinceDate: new Date(),
        cclPurchase: 1033,
        quantity: 325,
        currentPriceUsd: 99,
        currentValueUsd: 99,
        pricePurchase: 89,
        pricePurchaseUsd: 11,
        valuePurchaseUsd: 231,
      },
    ],
  },
  {
    ticker: "GOOGL",
    name: "ALPHABET INC.",
    quantity: 220,
    ratio: 58,
    value: 9673,
    todayChange: -61.26,
    todayChangePercent: -0.6,
    sinceChange: -331.94,
    sinceChangePercent: -3.3,
    details: [
      {
        ticker: "GOOGL",
        sinceDate: new Date(),
        cclPurchase: 1660,
        quantity: 20,
        currentPriceUsd: 99,
        currentValueUsd: 99,
        pricePurchase: 89,
        pricePurchaseUsd: 11,
        valuePurchaseUsd: 333,
      },
    ],
  },
];

export default function Dashboard() {
  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {}
  );

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
    <div className="flex flex-col min-h-screen">
      <main className="flex-1 container mx-auto p-6">
        <SummaryPortfolio
          portfolioValue={portfolioValue}
          todaysGain={todaysGain}
          todaysGainPercent={todaysGainPercent}
        />
        <CedearsInfoTabs
          cedears={cedears}
          expandedTicker={expandedTicker}
          toggleCedear={toggleCedear}
        />
      </main>
    </div>
  );
}
