"use client";

import { useEffect, useState } from "react";
import SummaryPortfolio from "../summary-portfolio/summary-portfolio";
import { getCedearStockHoldingAsync } from "@/api/cedears-api";
import { Cedears, CedearsStockResponse } from "@/types/cedears";
import { AmountPercentage } from "@/types/amount-percentage";
import { User } from "@/types/user";
import UserProfile from "../user-profile/user-profile";
import { PieChart, PlusCircle } from "lucide-react";
import { Button } from "../ui/button";
import { useNavigate } from "react-router-dom";
import { LoaderSkeleton } from "../loader-skeleton/loader-skeleton";
import EmptyStatePortfolio from "../summary-portfolio/empty-state-portfolio";
import CedearsInfoTabs from "../cedears-info/cedears-info-tabs";
import { useAuth } from "@/hooks/useAuth";
import { setApiToken } from "@/lib/utils";
import { useData } from "@/hooks/useData";
import { useWebSocket } from "@/hooks/useWebSocket";

export default function Dashboard() {
  const { logout, getCurrentUser } = useAuth();
  const [cedearsStockResponse, setCedearsStockHolding] =
    useState<CedearsStockResponse>();
  const { refreshData, dollarCCLQuote } = useData();
  const { client } = useWebSocket();
  const [loading, setLoading] = useState<boolean>(true);
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);
  const [expandedTicker, setExpandedTicker] = useState<Record<string, boolean>>(
    {}
  );

  const totalGainLoss: AmountPercentage = {
    amount: dollarCCLQuote ? dollarCCLQuote.dollarCCL : 0,
    percentage: dollarCCLQuote ? dollarCCLQuote.variationCCL : 0,
  };

  useEffect(() => {
    refreshData();
  }, [refreshData]);

  useEffect(() => {
    getUser();
  }, []);

  useEffect(() => {
    if (!client) return;

    let isSubscribed = false;
    let unsubscribe: (() => void) | undefined;

    const subscribe = async () => {
      unsubscribe = await client.subscribe(
        (updatedStocksHolding: CedearsStockResponse) => {
          updateCedearStockHoldingPrice(
            updatedStocksHolding.cedearWithStockHoldings
          );
        }
      );

      isSubscribed = true;
    };

    subscribe();

    return () => {
      if (unsubscribe && isSubscribed) {
        unsubscribe();
      }
    };
  }, [client]);

  const getUser = async () => {
    return await getCurrentUser().then(async (currentUser) => {
      setUser(currentUser);
      setApiToken(currentUser!.token!, currentUser!.refresh_token!);
      loadCedears(currentUser!.id);
    });
  };

  const loadCedears = async (userId: string) => {
    try {
      setLoading(true);
      await getCedearStockHoldingAsync(userId)
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
  };

  const updateCedearWithPriceChangeDirection = (
    updatedList: Cedears[],
    prev: CedearsStockResponse
  ): Cedears[] => {
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
        ...updated,
        priceChangeDirection,
      };
    });

    return updatedCedears;
  };

  const updateCedearStockHoldingPrice = (updatedList: Cedears[]) => {
    setCedearsStockHolding((prev) => {
      if (!prev) return prev;
      const updatedCedears = updateCedearWithPriceChangeDirection(
        updatedList,
        prev
      );

      resetPriceChangeColor();

      return {
        ...prev,
        cedearWithStockHoldings: updatedCedears,
      };
    });
  };

  const toggleCedear = (cedearsId: string) => {
    setExpandedTicker((prev) => ({
      ...prev,
      [cedearsId]: !prev[cedearsId],
    }));
  };

  const getTotalPorfolioValue = (
    cedearsStockResponse: CedearsStockResponse
  ): number => {

    return cedearsStockResponse.cedearWithStockHoldings.reduce(
      (total, cedear) => {
        const totalPorCedear = cedear.cedearsStockHoldings.reduce(
          (subtotal, holding) => {
            return subtotal + holding.currentValueUsd;
          },
          0
        );
        return total + totalPorCedear;
      },
      0
    );
  };

  const handleLogout = () => {
    logout();
    setUser(null);
    navigate("/");
  };

  return (
    <>
      <div className="space-y-8">
        <div className="min-h-screen bg-gradient-to-br from-slate-50 to-slate-100">
          <header className="bg-white border-b border-slate-200 shadow-sm">
            <div className="max-w-[90rem] mx-auto px-4 sm:px-6 lg:px-8">
              <div className="flex justify-between items-center h-20">
                <div className="flex items-center space-x-4">
                  <div className="flex items-center space-x-2">
                    <div className="w-8 h-8 bg-gradient-to-r from-emerald-500 to-blue-600 rounded-lg flex items-center justify-center">
                      <PieChart className="w-5 h-5 text-white" />
                    </div>
                    <h1 className="text-xl font-bold text-slate-900">
                      Calculadora CEDEARs
                    </h1>
                  </div>
                </div>

                <div className="flex items-center space-x-4">
                  <UserProfile user={user} onLogout={handleLogout} />
                </div>
              </div>
            </div>
          </header>

          <main className="max-w-[90rem] mx-auto px-4 sm:px-6 lg:px-8 py-6">
            <SummaryPortfolio
              portfolioValue={
                cedearsStockResponse != undefined &&
                cedearsStockResponse?.cedearWithStockHoldings.length > 0
                  ? getTotalPorfolioValue(cedearsStockResponse)
                  : 0
              }
              dolarCCL={totalGainLoss}
              loading={loading}
            />

            {!loading &&
            cedearsStockResponse != undefined &&
            cedearsStockResponse?.cedearWithStockHoldings.length > 0 ? (
              <div className="flex items-center justify-between mb-6">
                <h3 className="text-xl font-semibold text-slate-900">
                  Mis CEDEARs
                </h3>
                <Button
                  className="bg-emerald-600 hover:bg-emerald-700 cursor-pointer"
                  onClick={() => navigate("/cedear")}
                >
                  <PlusCircle className="w-4 h-4 mr-2" />
                  Agregar CEDEAR
                </Button>
              </div>
            ) : (
              <span></span>
            )}

            <>
              {loading ? (
                <>
                  <LoaderSkeleton rows={5} />
                </>
              ) : !loading &&
                cedearsStockResponse != undefined &&
                cedearsStockResponse?.cedearWithStockHoldings.length === 0 ? (
                <>
                  <EmptyStatePortfolio />
                </>
              ) : (
                <>
                  <CedearsInfoTabs
                    cedears={cedearsStockResponse?.cedearWithStockHoldings}
                    expandedTicker={expandedTicker}
                    toggleCedear={toggleCedear}
                    onRefresh={() => loadCedears(user!.id)}
                  />
                </>
              )}
            </>
          </main>
        </div>
      </div>
    </>
  );
}
