export type GainLossPortfolioProps = {
    portfolioValue: number;
    todayGainLoss: {
      amount: number;
      percentage: number;
    };
    dolarCCL: {
      amount: number;
      percentage: number;
    };
    hasCedears: boolean;
  };