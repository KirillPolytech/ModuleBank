using BankAccount.Features.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAccount.Persistence.Db.EntityTypeConfigurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.AccountId)
            .IsRequired()
            .HasColumnName("account_id");

        builder.Property(t => t.CounterpartyAccountId)
            .HasColumnName("counterparty_account_id");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("numeric(18,2)")
            .HasColumnName("amount");

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasColumnName("currency");

        builder.Property(t => t.Type)
            .IsRequired()
            .HasColumnName("type");

        builder.Property(t => t.Description)
            .IsRequired()
            .HasColumnName("description");

        builder.Property(t => t.Timestamp)
            .IsRequired()
            .HasColumnName("timestamp");

        builder.HasOne<Account>()
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.CounterpartyAccountId);

        builder.HasIndex(t => new { t.AccountId, t.Timestamp })
            .HasDatabaseName("ix_transactions_accountid_timestamp");

        builder.HasIndex(t => t.Timestamp)
            .HasDatabaseName("ix_transactions_timestamp_btree");
    }
}