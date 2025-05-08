"use client";

import { useEffect, useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import { getCedearStockHoldingAsync } from "@/api/cedears-api";
import { Cedears, CedearsStockResponse } from "@/types/cedears";
import { wsClient } from "@/services/web-socket.client";
import { AmountPercentage } from "@/types/amount-percentage";

export default function Dashboard() {
  const [cedearsStockResponse, setCedearsStockHolding] =
    useState<CedearsStockResponse>();
  const [loading, setLoading] = useState<boolean>(true);

  const portfolioValue = 99999;
  const totalGainLoss:AmountPercentage = {
    amount: 999.99,
    percentage:9.99
  }

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

    const unsubscribe = wsClient.subscribe((updatedStocksHolding: CedearsStockResponse) => {
        updateCedearStockHoldingPrice(updatedStocksHolding.cedearWithStockHoldings);
      }
    );

    return () => {
      unsubscribe();
    };
  }, [loading]);


  const resetPriceChangeColor = () => {

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

  }

  const updateCedearWithPriceChangeDirection = (updatedList: Cedears[], prev:CedearsStockResponse) : Cedears[] => {

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

    return updatedCedears;
  }

  const updateCedearStockHoldingPrice = (updatedList: Cedears[]) => {

    setCedearsStockHolding((prev) => {
      if (!prev) return prev;
      
      const updatedCedears = updateCedearWithPriceChangeDirection(updatedList, prev);

      resetPriceChangeColor();

      return {
        ...prev,
        cedearWithStockHoldings: updatedCedears,
      };
    });
  };

  return (
    <>
      <SummaryPortfolio
        portfolioValue={portfolioValue}        
        dolarCCL={totalGainLoss}
        cedears={cedearsStockResponse?.cedearWithStockHoldings}
        onRefresh={loadCedears}
        loading={loading}
      />
    </>
  );
}
