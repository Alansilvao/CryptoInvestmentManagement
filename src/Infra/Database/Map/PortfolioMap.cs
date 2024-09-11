using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Map;

[ExcludeFromCodeCoverage]
public class PortfolioMap : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("PORTFOLIO");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("COD_PORTFOLIO");

        builder.Property(x => x.AccountId)
            .HasColumnName("COD_CONTA")
            .IsRequired();

        builder.Property(x => x.AssetId)
           .HasColumnName("COD_ATIVO")
           .IsRequired();

        builder.Property(x => x.Symbol)
            .HasColumnName("NOM_SIMBOLO")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("VAL_QUANTIDADE")
            .IsRequired();

        builder.Property(x => x.AveragePurchasePrice)
            .HasColumnName("VAL_PRECO_MEDIO")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.AcquisitionValue)
            .HasColumnName("VAL_AQUISICAO")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CurrentValue)
           .HasColumnName("VAL_ATUAL")
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(x => x.CreatedAt)
           .HasColumnName("DAT_INC")
           .IsRequired();

        builder.Property(x => x.UpdatedAt)
           .HasColumnName("DAT_ALT")
           .IsRequired();
    }
}
