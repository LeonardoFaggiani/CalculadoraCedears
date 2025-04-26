import { Card } from "../ui/card";
import { cn } from "@/lib/utils";

export function LoaderSkeleton() {
  // Crear un array de 5 elementos para mostrar 5 filas de skeleton
  const skeletonRows = Array.from({ length: 6 }, (_, i) => i);

  return (
    <div className="space-y-4">
      {skeletonRows.map((index) => (
        <Card key={index} className="p-6">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <Skeleton className="h-4 w-4 rounded-full" />
              <Skeleton className="h-5 w-40" />
              <Skeleton className="h-5 w-16 ml-2" />
              <Skeleton className="h-5 w-24 ml-2" />
            </div>
            <div className="flex items-center space-x-8">
              <div>
                <Skeleton className="h-3 w-32 mb-1" />
                <Skeleton className="h-5 w-20" />
              </div>
              <div>
                <Skeleton className="h-3 w-32 mb-1" />
                <Skeleton className="h-5 w-20" />
              </div>
              <div>
                <Skeleton className="h-3 w-32 mb-1" />
                <Skeleton className="h-5 w-24" />
              </div>
            </div>
          </div>
        </Card>
      ))}
    </div>
  );
}

function Skeleton({
  className,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) {
  return (
    <div
      className={cn("animate-pulse rounded-md bg-primary/10", className)}
      {...props}
    />
  );
}

export { Skeleton };
