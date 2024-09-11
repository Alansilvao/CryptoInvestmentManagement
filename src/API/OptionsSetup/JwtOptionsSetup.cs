using Infra.Authentication;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace API.OptionsSetup;

[ExcludeFromCodeCoverage]
public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
	private readonly IConfiguration _configuration;

	public JwtOptionsSetup(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void Configure(JwtOptions options)
	{
		_configuration.GetSection("JwtSettings").Bind(options);
	}
}