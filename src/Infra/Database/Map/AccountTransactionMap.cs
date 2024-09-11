using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Map;

[ExcludeFromCodeCoverage]
public class AccountTransactionMap : IEntityTypeConfiguration<AccountTransaction>
{
    public void Configure(EntityTypeBuilder<AccountTransaction> builder)
    {
        builder.ToTable("TRANSACAO_CONTA");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("COD_TRANSACAO_CONTA");

        builder.Property(x => x.AccountId)
            .HasColumnName("COD_CONTA")
            .IsRequired();

        builder.Property(x => x.TransactionType)
            .HasColumnName("COD_TIPO_TRANSACAO")
            .IsRequired();

        builder.Property(x => x.Value)
           .HasColumnName("VAL_TRANSACAO")
           .HasPrecision(18, 2)
           .IsRequired();

        builder.Property(x => x.CreatedAt)
           .HasColumnName("DAT_INC")
           .IsRequired();
    }
}
