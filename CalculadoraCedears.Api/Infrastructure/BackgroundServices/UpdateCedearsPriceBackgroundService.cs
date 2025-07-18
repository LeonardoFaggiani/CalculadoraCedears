using AutoMapper;

using CalculadoraCedears.Api.Dto.CedearsStockHolding;
using CalculadoraCedears.Api.Infrastructure.Exceptions;
using CalculadoraCedears.Api.Infrastructure.Extensions;
using CalculadoraCedears.Api.Infrastructure.Repositories;
using CalculadoraCedears.Api.Infrastructure.WebSocket;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

namespace CalculadoraCedears.Api.Infrastructure.BackgroundServices
{
    public class UpdateCedearsPriceBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public UpdateCedearsPriceBackgroundService(IServiceScopeFactory scopeFactory)
        {
            Guard.IsNotNull(scopeFactory, nameof(scopeFactory));

            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = scopeFactory.CreateScope();
                var googleRepository = scope.ServiceProvider.GetRequiredService<IGoogleRepository>();
                var cedearRepository = scope.ServiceProvider.GetRequiredService<ICedearRepository>();
                var cedearsStockHoldingUpdateService = scope.ServiceProvider.GetRequiredService<ICedearsStockHoldingUpdateService>();
                var cedearStockHoldingRepository = scope.ServiceProvider.GetRequiredService<ICedearStockHoldingRepository>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var cedears = await cedearRepository.All().ToListAsync(cancellationToken);

                foreach (var cedear in cedears)
                {
                    try
                    {
                        var googleFinance = await googleRepository
                            .TryGetFromFinanceCurrentPriceByTickerAndMarketAsync(cedear.Ticker, cedear.Market, cancellationToken);

                        cedear.SetPriceHasBeenChanged(Math.Round(googleFinance.Price, 2, MidpointRounding.AwayFromZero) != cedear.Price);
                        cedear.SetPrice(Math.Round(googleFinance.Price, 2, MidpointRounding.AwayFromZero));
                        cedearRepository.Update(cedear);
                    }
                    catch (GoogleFinancePriceNotFoundException)
                    {
                        continue;
                    }
                }

                await cedearRepository.UnitOfWork.Commit();

                foreach (var client in cedearsStockHoldingUpdateService.Clients)
                {
                    var cedearsByTicker = await cedearStockHoldingRepository.GetActivesAndGroupedByTickerAsync(client.Key, cancellationToken, true);

                    if (cedearsByTicker.Any())
                    {
                        var result = new CedearsStockHoldingQueryResponse(cedearsByTicker.ConvertToResult(mapper));

                        var cedearsStockHoldings = cedearsByTicker.Values.SelectMany(lista => lista).ToList();

                        foreach (var cedearsStockHolding in cedearsStockHoldings)
                            cedearStockHoldingRepository.Update(cedearsStockHolding);

                        await cedearStockHoldingRepository.UnitOfWork.Commit();
                        await cedearsStockHoldingUpdateService.BroadcastCedearsStockHoldingUpdatesAsync(client.Value, result);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }
    }
}