import { Tabs, TabsContent } from "@/components/ui/tabs";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { ChevronDown, ChevronUp } from "lucide-react";
import CedearsDetailTable from "../cedears-details/cedears-detail-table";
import PriceGainLoss from "../price-gain-loss/price-gain-loss";
import { Cedears } from "@/types/cedears";
import { CedearsInfoTab } from "@/types/cedears-info-tabs";
import { getTotalChangeSummary } from "@/lib/utils";

export default function CedearsInfoTabs({
  cedears,
  expandedTicker,
  toggleCedear,
  onRefresh,
}: CedearsInfoTab) {
  return (
    <Tabs defaultValue="summary" className="mb-6">
      <TabsContent value="summary" className="mt-0">
        <div className="space-y-4">
          {cedears?.map((cedear: Cedears) => (
            <Card key={cedear.ticker} className="overflow-hidden">
              <Collapsible
                open={expandedTicker[cedear.ticker]}
                onOpenChange={() => toggleCedear(cedear.ticker)}
              >
                <CollapsibleTrigger asChild>
                  <div className="flex items-center justify-between pr-6 pl-6 cursor-pointer">
                    <div className="flex items-center space-x-2">
                      {expandedTicker[cedear.ticker] ? (
                        <ChevronDown className="w-5 h-5 text-gray-500" />
                      ) : (
                        <ChevronUp className="w-5 h-5 text-gray-500" />
                      )}
                      <h3 className="font-medium">{cedear.name}</h3>
                      <Badge variant="outline" className="text-xs">
                        {cedear.ticker}
                      </Badge>
                      <Badge variant="outline" className="text-xs">
                        Ratio {cedear.ratio}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-8">
                      <div>
                        <div className="text-xs text-gray-500">
                          TOTAL VALOR COMPRA (U$S)
                        </div>
                        <div>
                          {getTotalChangeSummary(cedear).totalPurchase}
                        </div>
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">
                          TOTAL VALOR ACTUAL (U$S)
                        </div>
                        <div className={`text-center transition duration-500 ${cedear.priceChangeDirection === "up" ? "text-green-100"  : cedear.priceChangeDirection === "down" ? "text-red-100" : ""}`}> {getTotalChangeSummary(cedear).totalCurrent}</div>
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">
                          DESDE LA COMPRA (U$S)
                        </div>
                        <PriceGainLoss
                          isPercent={false}
                          value={getTotalChangeSummary(cedear).sinceChange}
                          percent={
                            getTotalChangeSummary(cedear).sinceChangePercent
                          }
                        />
                      </div>
                    </div>
                  </div>
                </CollapsibleTrigger>
                <CollapsibleContent>
                  <CardContent className="p-4">
                    <CedearsDetailTable cedear={cedear} onRefresh={onRefresh} />
                  </CardContent>
                </CollapsibleContent>
              </Collapsible>
            </Card>
          ))}
        </div>
      </TabsContent>
    </Tabs>
  );
}
