import { formatPercent } from "@/lib/utils";

export default function PriceGainLoss({
  value,
  percent = null,
  isPercent = false
}: PriceGainLossChangeValue) {
  let color = "text-orange-500";
  if (value > 0) color = "text-green-500";
  else if (value < 0) color = "text-red-500";


  const symbol = value > 0 ? "▲" : value < 0 ? "▼" : "=";
  const showOnlyValue = percent == null;

  const formattedValue = isPercent
    ? `${value.toFixed(2)}%`
    : `$${value.toLocaleString()}`;

  return (
    <div className={color}>
      {formattedValue}
      {!showOnlyValue && (
        <>        
          {" "}{formatPercent(percent)} {symbol}
        </>
      )}
    </div>
  );
}