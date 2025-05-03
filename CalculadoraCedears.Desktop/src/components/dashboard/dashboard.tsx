"use client";

import { useEffect, useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";
import { getCedearStockHoldingAsync } from "@/api/cedears-api";
import { Cedears, CedearsStockResponse } from "@/types/cedears";
import { wsClient } from "@/services/web-socket.client";

export default function Dashboard() {
  const [cedearsStockResponse, setCedearsStockHolding] =
    useState<CedearsStockResponse>();
  const [loading, setLoading] = useState<boolean>(true);

  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {}
  );

  useEffect(() => {
    loadCedears();
  }, []);

  const loadCedears = async () => {
    try {
      setLoading(true);

      await getCedearStockHoldingAsync()
        .then(setCedearsStockHolding)
        .catch(console.log);
    } catch (err) {
      console.error(err);
    } finally {
      setTimeout(() => {
        setLoading(false);
      }, 1000);
    }
  };

  useEffect(() => {
    if (loading) return;

    const unsubscribe = wsClient.subscribe(
      (updatedStocksHolding: CedearsStockResponse) => {
        updateCedearStockHoldingPrice(
          updatedStocksHolding.cedearWithStockHoldings
        );
        console.log(updatedStocksHolding.cedearWithStockHoldings);
      }
    );

    return () => {
      unsubscribe();
    };
  }, [loading]);

  const toggleCedear = (cedearsId: string) => {
    setExpandedTicker((prev) => ({
      ...prev,
      [cedearsId]: !prev[cedearsId],
    }));
  };

  const updateCedearStockHoldingPrice = (updatedList: Cedears[]) => {
    setCedearsStockHolding((prev) => {
      if (!prev) return prev;

      const updatedCedears = prev.cedearWithStockHoldings.map((cedear) => {
        const updated = updatedList.find((u) => u.id === cedear.id);
        if (!updated) return cedear;

        const priceChangeDirection =
          updated.price > cedear.price
            ? "up"
            : updated.price < cedear.price
            ? "down"
            : "equal";

        return {
          ...cedear,
          price: updated.price,
          priceChangeDirection,
        };
      });

      // Reset priceChangeDirection después de 1s
      setTimeout(() => {
        setCedearsStockHolding((prevAfterTimeout) => {
          if (!prevAfterTimeout) return prevAfterTimeout;

          const resetCedears = prevAfterTimeout.cedearWithStockHoldings.map(
            (cedear) => ({
              ...cedear,
              priceChangeDirection: "equal",
            })
          );

          return {
            ...prevAfterTimeout,
            cedearWithStockHoldings: resetCedears,
          };
        });
      }, 1000);

      return {
        ...prev,
        cedearWithStockHoldings: updatedCedears,
      };
    });
  };

  const portfolioValue = 274980;
  const todaysGain = 706.22;
  const todaysGainPercent = 0.3;

  return (
    <>
      <SummaryPortfolio
        portfolioValue={portfolioValue}
        todaysGain={todaysGain}
        todaysGainPercent={todaysGainPercent}
      />
      <CedearsInfoTabs
        cedears={cedearsStockResponse?.cedearWithStockHoldings}
        expandedTicker={expandedTicker}
        toggleCedear={toggleCedear}
        onRefresh={loadCedears}
        loading={loading}
      />
    </>
  );
}
