
export type SummaryPortfolioProps = {
    portfolioValue: number;
    dolarCCL: {
      amount: number
      percentage: number
    },
    onRefresh: () => Promise<void>;
  };