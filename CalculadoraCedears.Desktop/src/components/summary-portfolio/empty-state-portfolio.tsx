import { PlusCircle, TrendingUp } from "lucide-react";
import { Card } from "../ui/card";
import { Button } from "../ui/button";
import { useNavigate } from "react-router-dom";

export default function EmptyStatePortfolio() {
  const navigate = useNavigate();

  return (
    <Card className="w-full p-8 flex flex-col items-center justify-center text-center bg-gradient-to-b from-background to-muted/30 border border-dashed">
      <div className="rounded-full bg-muted p-4 mb-4">
        <TrendingUp className="h-8 w-8 text-emerald-500" />
      </div>
      <h3 className="text-xl font-semibold mb-2">
        No tienes CEDEARs en tu calculadora
      </h3>
      <p className="text-muted-foreground mb-6 max-w-md">
        Comienza a construir tu calculadora agregando CEDEARs, para hacer un seguimiento de tus inversiones.
      </p>

      <Button
        onClick={() => navigate("/cedear")}
        className="bg-emerald-500 hover:bg-emerald-600 cursor-pointer"
      >
        <PlusCircle className="mr-2 h-4 w-4" />
        Agregar CEDEAR
      </Button>
    </Card>
  );
}
