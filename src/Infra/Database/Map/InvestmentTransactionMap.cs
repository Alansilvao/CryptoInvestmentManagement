using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Map;

[ExcludeFromCodeCoverage]
public class InvestmentTransactionMap : IEntityTypeConfiguration<InvestmentTransaction>
{
    public void Configure(EntityTypeBuilder<InvestmentTransaction> builder)
    {
        builder.ToTable("TRANSACAO_INVESTIMENTO");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("COD_TRANSACAO_INVESTIMENTO");

        builder.Property(x => x.AccountId)
            .HasColumnName("COD_CONTA")
            .IsRequired();

        builder.Property(x => x.AssetId)
            .HasColumnName("COD_ATIVO")
            .IsRequired();

        builder.Property(x => x.TransactionType)
            .HasColumnName("COD_TIPO_TRANSACAO")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("VAL_QUANTIDADE")
            .IsRequired();

        builder.Property(x => x.Price)
           .HasColumnName("VAL_PRECO")
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(x => x.CreatedAt)
           .HasColumnName("DAT_INC")
           .IsRequired();
    }
}
