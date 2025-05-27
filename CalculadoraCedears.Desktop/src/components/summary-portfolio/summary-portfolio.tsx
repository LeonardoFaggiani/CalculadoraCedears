import { SummaryPortfolioProps } from "@/types/summary-portfolio";
import { GainLossPortfolio } from "./gain-loss-portfolio";
import { Card, CardHeader } from "../ui/card";

export default function SummaryPortfolio({
  portfolioValue,
  dolarCCL,
  loading = false,
}: SummaryPortfolioProps & { loading?: boolean }) {
  return (
    <>
      {loading ? (
        <>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
            {/* Tu Portfolio Card Skeleton */}
            <Card className="md:col-span-1">
              <CardHeader className="pb-3">
                <div className="space-y-3">
                  <div className="h-4 w-20 bg-gradient-to-r from-gray-200 via-gray-300 to-gray-200 rounded relative overflow-hidden">
                    <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                  </div>
                  <div className="h-8 w-32 bg-gradient-to-r from-gray-300 via-gray-400 to-gray-300 rounded relative overflow-hidden">
                    <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                  </div>
                </div>
              </CardHeader>
            </Card>

            {/* Ganancia/Pérdida Total Card Skeleton */}
            <Card className="md:col-span-1">
              <CardHeader className="pb-3">
                <div className="flex items-center justify-between">
                  <div className="space-y-3">
                    <div className="h-4 w-36 bg-gradient-to-r from-gray-200 via-gray-300 to-gray-200 rounded relative overflow-hidden">
                      <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <div className="w-4 h-4 bg-gradient-to-r from-gray-300 via-gray-400 to-gray-300 rounded relative overflow-hidden">
                        <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                      </div>
                      <div className="h-6 w-40 bg-gradient-to-r from-gray-300 via-gray-400 to-gray-300 rounded relative overflow-hidden">
                        <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                      </div>
                    </div>
                  </div>
                </div>
              </CardHeader>
            </Card>

            {/* Dólar CCL Card Skeleton */}
            <Card>
              <CardHeader className="pb-3">
                <div className="flex items-center justify-between">
                  <div className="space-y-3">
                    <div className="h-4 w-16 bg-gradient-to-r from-gray-200 via-gray-300 to-gray-200 rounded relative overflow-hidden">
                      <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                    </div>
                    <div className="flex items-center space-x-2">
                      <div className="w-4 h-4 bg-gradient-to-r from-gray-300 via-gray-400 to-gray-300 rounded relative overflow-hidden">
                        <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                      </div>
                      <div className="h-6 w-32 bg-gradient-to-r from-gray-300 via-gray-400 to-gray-300 rounded relative overflow-hidden">
                        <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                      </div>
                    </div>
                  </div>
                  <div className="w-8 h-8 bg-gradient-to-r from-gray-200 via-gray-300 to-gray-200 rounded-full relative overflow-hidden">
                    <div className="absolute inset-0 -translate-x-full animate-[shimmer_1.5s_infinite] bg-gradient-to-r from-transparent via-white/30 to-transparent"></div>
                  </div>
                </div>
              </CardHeader>
            </Card>
          </div>
        </>
      ) : (
        <>
          <GainLossPortfolio
            portfolioValue={portfolioValue}
            dolarCCL={dolarCCL}
          />
        </>
      )}
    </>
  );
}
