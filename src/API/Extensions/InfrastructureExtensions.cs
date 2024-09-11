using System.Diagnostics.CodeAnalysis;
using Application.Interfaces.Repositories;
using Infra.Database.Context;
using Infra.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

[ExcludeFromCodeCoverage]
public static class InfrastructureExtensions
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext(configuration);
		services.AddRepositories();
	}

	private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("SqlServer");

		services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
	}

	private static void AddRepositories(this IServiceCollection services)
	{
		services
			.AddScoped<IClientsRepository, ClientsRepository>()
			.AddScoped<IAccountsRepository, AccountsRepository>()
			.AddScoped<IAssetsRepository, AssetsRepository>()
			.AddScoped<IPortfolioRepository, PortfolioRepository>();
	}
}
