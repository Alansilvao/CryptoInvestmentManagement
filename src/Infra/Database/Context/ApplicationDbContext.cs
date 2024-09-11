using Domain.Entities;
using Infra.Database.Map;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Context;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
	}

    public DbSet<Asset> Assets { get; set; }
    public DbSet<Client> Clients { get; set; }
	public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountTransaction> AccountTransactions { get; set; }
    public DbSet<InvestmentTransaction> InvestmentTransactions { get; set; }
	public DbSet<Portfolio> Portfolios { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.ApplyConfiguration(new AssetMap());
        modelBuilder.ApplyConfiguration(new ClientMap());
        modelBuilder.ApplyConfiguration(new AccountMap());
        modelBuilder.ApplyConfiguration(new AccountTransactionMap());
        modelBuilder.ApplyConfiguration(new InvestmentTransactionMap());
        modelBuilder.ApplyConfiguration(new PortfolioMap());

        base.OnModelCreating(modelBuilder);       
	}
}