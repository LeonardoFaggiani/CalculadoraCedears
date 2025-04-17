"use client";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";

export default function Dashboard() {
    
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
          todaysGainPercent={todaysGainPercent} />

      </main>
    </div>
  );
}
