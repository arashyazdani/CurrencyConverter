using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.FromCurrency).IsRequired().HasMaxLength(7);
            builder.Property(p => p.ToCurrency).IsRequired().HasMaxLength(7);
            builder.Property(p => p.Rate).HasColumnType("decimal(10,7)");
        }
    }
}
