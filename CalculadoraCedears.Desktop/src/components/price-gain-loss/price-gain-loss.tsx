import { formatPercent } from "@/lib/utils";

export default function PriceGainLoss({
  value,
  percent
}: PriceGainLossChangeValue) {

  let color = "text-orange-500";
  if (value > 0) color = "text-green-500";
  else if (value < 0) color = "text-red-500";

  const symbol = value > 0 ? "▲" : value < 0 ? "▼" : "=";

  return (
    <div className={color}>
      ${value.toLocaleString()}{" "}
      {formatPercent(percent)} {symbol}
    </div>
  );

}
