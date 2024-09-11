using System.Diagnostics.CodeAnalysis;
using System.Text;
using Infra.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.OptionsSetup;

[ExcludeFromCodeCoverage]
public class JwtBearerOptionsSetup : IConfigureOptions<JwtBearerOptions>
{
	private readonly JwtOptions _jwtOptions;

	public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.Value;
	}

	public void Configure(JwtBearerOptions options)
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidIssuer = _jwtOptions.Issuer,
			ValidAudience = _jwtOptions.Audience,
			IssuerSigningKey = new SymmetricSecurityKey
				(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true
		};
	}
}