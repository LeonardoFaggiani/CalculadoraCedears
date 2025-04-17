import { Tabs, TabsContent } from "@/components/ui/tabs";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { ChevronDown, ChevronUp } from "lucide-react";
import { formatPercent } from "@/lib/utils";
import CedearsDetailTable from "../cedears-details/cedears-detail-table";

interface CedearInfoTabsProps {
  cedears: Cedears[];
  expandedTicker: Record<string, boolean>;
  toggleCedear: (id: string) => void;
}

export default function CedearsInfoTabs({
  cedears,
  expandedTicker,
  toggleCedear,
}: CedearInfoTabsProps) {
  return (
    <Tabs defaultValue="summary" className="mb-6">
      <TabsContent value="summary" className="mt-0">
        <div className="space-y-4">
          {cedears.map((cedear) => (
            <Card key={cedear.id} className="overflow-hidden">
              <Collapsible
                open={expandedTicker[cedear.id]}
                onOpenChange={() => toggleCedear(cedear.id)}
              >
                <CollapsibleTrigger asChild>
                  <div className="flex items-center justify-between p-1 cursor-pointer hover:bg-gray-50">
                    <div className="flex items-center space-x-2">
                      {expandedTicker[cedear.id] ? (
                        <ChevronDown className="w-5 h-5 text-gray-500" />
                      ) : (
                        <ChevronUp className="w-5 h-5 text-gray-500" />
                      )}
                      <h3 className="font-medium">
                        {cedear.name} *{cedear.accountNumber}
                      </h3>
                      <Badge variant="outline" className="ml-2 text-xs">
                        {cedear.syncStatus}
                      </Badge>
                    </div>
                    <div className="flex items-center space-x-8">
                      <div>
                        <div className="text-xs text-gray-500">VALUE</div>
                        <div>${cedear.value?.toLocaleString()}</div>
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">TODAY</div>
                        <div className="text-red-500">
                          ${cedear.todayChange?.toLocaleString()}{" "}
                          {formatPercent(cedear.todayChangePercent || 0)} ▼
                        </div>
                      </div>
                      <div>
                        <div className="text-xs text-gray-500">
                          SINCE PURCHASE
                        </div>
                        <div className="text-red-500">
                          ${cedear.sinceChange?.toLocaleString()}{" "}
                          {formatPercent(cedear.sinceChangePercent || 0)} ▼
                        </div>
                      </div>
                    </div>
                  </div>
                </CollapsibleTrigger>
                <CollapsibleContent>
                  <CardContent className="p-0">
                    <CedearsDetailTable holdings={cedear.holdings} />
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