﻿using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;

using NetDevPack.Data;
using NetDevPack.Domain;
using NetDevPack.Mediator;
using NetDevPack.Messaging;

using CaluladoraCedears.Api.Domian;
using CaluladoraCedears.Api.Infrastructure.Extensions;

namespace CaluladoraCedears.Api.Infrastructure.Data
{
    public class CaluladoraCedearsContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler mediatorHandler;

        public CaluladoraCedearsContext(DbContextOptions<CaluladoraCedearsContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
            Guard.IsNotNull(mediatorHandler, nameof(mediatorHandler));

            this.mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Commit()
        {
            var success = await SaveChangesAsync() > 0;

            await this.mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            return success;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Event>();

            modelBuilder.Entity<Entity>().HasKey(e => e.Id);

            modelBuilder.Entity<Cedear>(entity =>
            {
                entity.Property(c => c.Id)
                .HasColumnName("Id");

                entity.ToTable("Cedears");

                entity.HasIndex(e => e.Description, "UK_Sample_Ticker").IsUnique();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ticker)
                .IsRequired()
                .HasMaxLength(5);
            });
        }

        public DbSet<Cedear> Samples { get; set; }
    }
}