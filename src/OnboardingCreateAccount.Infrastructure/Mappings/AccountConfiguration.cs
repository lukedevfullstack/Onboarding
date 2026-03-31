using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingCreateAccount.Domain.Entities;

namespace OnboardingCreateAccount.Infrastructure.Mappings;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnType("CHAR(36)")
            .IsRequired();

        builder.Property(a => a.OwnerName)
            .HasMaxLength(150)
            .IsRequired()
            .HasColumnName("OwnerName");

        builder.Property(a => a.Document)
            .HasMaxLength(14)
            .IsRequired()
            .HasColumnName("Document");

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("IsActive");

        builder.HasIndex(a => a.Document)
            .IsUnique()
            .HasDatabaseName("IX_Account_Document");
    }
}
