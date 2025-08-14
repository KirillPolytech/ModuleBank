using BankAccount.Features.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAccount.Persistence.Db.EntityTypeConfigurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.OwnerId)
            .IsRequired()
            .HasColumnName("owner_id");

        builder.Property(a => a.Type)
            .IsRequired()
            .HasColumnName("type");

        builder.Property(a => a.CurrencyType)
            .IsRequired()
            .HasColumnName("currency_type");

        builder.Property(a => a.Balance)
            .HasColumnType("numeric(18,2)")
            .HasColumnName("balance");

        builder.Property(a => a.InterestRate)
            .HasColumnType("numeric(5,4)")
            .HasColumnName("interest_rate");

        builder.Property(a => a.OpenDate)
            .IsRequired()
            .HasColumnName("open_date");

        builder.Property(a => a.CloseDate)
            .HasColumnName("close_date");

        builder.HasIndex(a => a.OwnerId)
            .HasDatabaseName("ix_accounts_ownerid_hash")
            .HasMethod("hash");
    }
}