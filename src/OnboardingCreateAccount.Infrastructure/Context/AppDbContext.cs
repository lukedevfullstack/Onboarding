using Microsoft.EntityFrameworkCore;
using OnboardingCreateAccount.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingCreateAccount.Infrastrsucture.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.OwnerName).IsRequired().HasMaxLength(150);
            builder.Property(a => a.Document).IsRequired().HasMaxLength(14);
            builder.HasIndex(a => a.Document).IsUnique();
        });
    }
}