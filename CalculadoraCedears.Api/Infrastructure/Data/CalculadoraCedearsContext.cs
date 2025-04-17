using CalculadoraCedears.Api.Domian;
using CalculadoraCedears.Api.Infrastructure.Extensions;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Domain;
using NetDevPack.Mediator;
using NetDevPack.Messaging;

namespace CalculadoraCedears.Api.Infrastructure.Data
{
    public class CalculadoraCedearsContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler mediatorHandler;

        public CalculadoraCedearsContext(DbContextOptions<CalculadoraCedearsContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            Guard.IsNotNull(mediatorHandler, nameof(mediatorHandler));

            this.mediatorHandler = mediatorHandler;
        }

        public virtual DbSet<Broker> Brokers { get; set; }

        public virtual DbSet<Cedear> Cedears { get; set; }

        public virtual DbSet<CedearsStockHolding> CedearsStockHoldings { get; set; }


        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            await mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            return success;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);

            modelBuilder.Entity<Broker>(entity =>
            {
                entity.HasIndex(e => e.Name, "UK_Brokers").IsUnique();

                entity.Property(e => e.Comision).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.Name).HasMaxLength(25);
            });

            modelBuilder.Entity<Cedear>(entity =>
            {
                entity.HasIndex(e => e.Ticker, "UK_Cedears").IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Ticker)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CedearsStockHolding>(entity =>
            {
                entity.ToTable("CedearsStockHolding");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CurrentPriceUsd).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.CurrentValueUsd).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.ExchangeRateCcl)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("ExchangeRateCCL");
                entity.Property(e => e.PurchasePriceArs).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PurchasePriceUsd).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.PurchaseValueUsd).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.EffectiveRatio).HasColumnType("decimal(4, 2)");
                entity.Property(e => e.SinceChange).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.SinceChangePercent).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.SinceDate).HasColumnType("datetime");
                entity.Property(e => e.TodayChange).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.TodayChangePercent).HasColumnType("decimal(5, 2)");
                entity.Property(e => e.UntilDate).HasColumnType("datetime");

                entity.HasOne(d => d.Broker).WithMany(p => p.CedearsStockHoldings)
                    .HasForeignKey(d => d.BrokerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CedearsStockHolding_Brokers");

                entity.HasOne(d => d.Cedear).WithMany(p => p.CedearsStockHoldings)
                    .HasForeignKey(d => d.CedearId)
                    .HasConstraintName("FK_CedearsStockHolding_Cedears");
            });
        }
    }
}