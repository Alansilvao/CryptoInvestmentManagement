using Infra.Database.Config;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace API.OptionsSetup;

[ExcludeFromCodeCoverage]
public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
	private readonly IConfiguration _configuration;

	public DatabaseOptionsSetup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void Configure(DatabaseOptions options)
	{
		_configuration.GetSection("ConnectionStrings").Bind(options);
	}
}
