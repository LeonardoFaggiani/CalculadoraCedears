import { Cedears } from "./cedears";

export type SummaryPortfolioProps = {
    cedears: Cedears[] | undefined
    portfolioValue: number;
    todayGainLoss: {
      amount: number
      percentage: number
    }
    dolarCCL: {
      amount: number
      percentage: number
    },
    onRefresh: () => Promise<void>;
  };