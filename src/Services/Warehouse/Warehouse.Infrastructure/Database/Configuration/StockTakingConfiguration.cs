﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using System;

namespace Restmium.ERP.Services.Warehouse.Infrastructure.Database.Configuration
{
    public class StockTakingConfiguration : IEntityTypeConfiguration<StockTaking>
    {
        public void Configure(EntityTypeBuilder<StockTaking> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UtcCreated)
                .HasDefaultValue(DateTime.UtcNow);
            builder.Property(x => x.UtcCreated)
                .ValueGeneratedOnAdd();
        }
    }
}
