"use client";

import { useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";

// Types for our data

// Sample data
const cedears: Cedears[] = [
  {
    id: "ameritrade",
    name: "Ameritrade Institutional Services",
    accountNumber: "4321",
    syncStatus: "SYNCED",
    value: 9673,
    todayChange: -61.26,
    todayChangePercent: -0.6,
    sinceChange: -331.94,
    sinceChangePercent: -3.3,
    holdings: [
      {
        ticker: "AGG",
        name: "iShares Core U.S. Aggregate Bond ETF",
        price: 108.84,
        change: -0.29,
        changePercent: -0.3,
        weekLow: 102.95,
        weekHigh: 109.55,
        currentPosition: 106.25,
        shares: 23,
        value: 2126535,
        totalGainLoss: -26535,
        totalGainLossPercent: -1.13,
      },
      {
        ticker: "BNDX",
        name: "Vanguard Total International Bond ETF",
        price: 51.03,
        change: -0.06,
        changePercent: -0.1,
        weekLow: 48.51,
        weekHigh: 51.75,
        currentPosition: 50.13,
        shares: 233,
        value: 2342,
        totalGainLoss: -2342,
        totalGainLossPercent: -0.1,
      },
      {
        ticker: "IVE",
        name: "iShares S&P 500 Value ETF",
        price: 89.1,
        change: 0.02,
        changePercent: 0.0,
        weekLow: 76.64,
        weekHigh: 91.74,
        currentPosition: 84.19,
        shares: 1652,
        value: 2143,
        totalGainLoss: 2143,
        totalGainLossPercent: 0.0,
      },
    ],
  },
  {
    id: "merrill",
    name: "Merrill Lynch",
    accountNumber: "1454",
    syncStatus: "MANUAL",
    value: 9673,
    todayChange: -61.26,
    todayChangePercent: -0.6,
    sinceChange: -331.94,
    sinceChangePercent: -3.3,
    holdings: [
      {
        ticker: "AGG",
        name: "iShares Core U.S. Aggregate Bond ETF",
        price: 108.84,
        change: -0.29,
        changePercent: -0.3,
        weekLow: 102.95,
        weekHigh: 109.55,
        currentPosition: 106.25,
        shares: 23,
        value: 2126535,
        totalGainLoss: -26535,
        totalGainLossPercent: -1.13,
      },
      {
        ticker: "BNDX",
        name: "Vanguard Total International Bond ETF",
        price: 51.03,
        change: -0.06,
        changePercent: -0.1,
        weekLow: 48.51,
        weekHigh: 51.75,
        currentPosition: 50.13,
        shares: 233,
        value: 2342,
        totalGainLoss: -2342,
        totalGainLossPercent: -0.1,
      },
    ],
  },
];

export default function Dashboard() {
  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {
      ameritrade: false,
      merrill: false,
    }
  );

  const toggleCedear = (cedearsId: string) => {
    setExpandedTicker((prev) => ({
      ...prev,
      [cedearsId]: !prev[cedearsId],
    }));
  };

  // Calculate portfolio totals
  const portfolioValue = 274980;
  const todaysGain = 706.22;
  const todaysGainPercent = 0.3;

  return (
    <div className="flex flex-col min-h-screen">
      {/* Main Content */}

      <main className="flex-1 container mx-auto p-6">
        {/* Portfolio Summary */}

        <SummaryPortfolio
          portfolioValue={portfolioValue}
          todaysGain={todaysGain}
          todaysGainPercent={todaysGainPercent}
        />

        {/* Tabs */}
        <CedearsInfoTabs
          cedears={cedears}
          expandedTicker={expandedTicker}
          toggleCedear={toggleCedear}
        />
      </main>
    </div>
  );
}
