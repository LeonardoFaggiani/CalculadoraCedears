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

export default function CedearsInfoTabs({
  cedears,
  expandedTicker,
  toggleCedear,
}: CedearsInfoTabs) {
  return (
    <Tabs defaultValue="summary" className="mb-6">
      <TabsContent value="summary" className="mt-0">
        <div className="space-y-4">
          {cedears.map((cedear) => (
            <Card key={cedear.ticker} className="overflow-hidden">
              <Collapsible
                open={expandedTicker[cedear.ticker]}
                onOpenChange={() => toggleCedear(cedear.ticker)}
              >
                <CollapsibleTrigger asChild>
                  <div className="flex items-center justify-between pr-6 pl-6 cursor-pointer hover:bg-gray-50">
                    <div className="flex items-center space-x-2">
                      {expandedTicker[cedear.ticker] ? (
                        <ChevronDown className="w-5 h-5 text-gray-500" />
                      ) : (
                        <ChevronUp className="w-5 h-5 text-gray-500" />
                      )}
                      <h3 className="font-medium">
                        {cedear.name}
                      </h3>
                      <Badge variant="outline" className="text-xs">
                        {cedear.ticker}
                      </Badge>
                      <Badge variant="outline" className="text-xs">
                        {cedear.ratio}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-8">
                      <div>
                        <div className="text-xs text-gray-500">PRECIO ACTUAL</div>
                        <div>${cedear.value?.toLocaleString()}</div>
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">HOY</div>
                        <PriceGainLoss value={cedear.todayChange}
                          percent={cedear.todayChangePercent}
                          />
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">
                          DESDE LA COMPRA
                        </div>
                        <PriceGainLoss  value={cedear.sinceChange}
                          percent={cedear.sinceChangePercent}
                          />
                      </div>
                    </div>
                  </div>
                </CollapsibleTrigger>
                <CollapsibleContent>
                  <CardContent className="p-4">
                    <CedearsDetailTable details={cedear.details} />
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