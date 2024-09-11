using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Authentication;

[ExcludeFromCodeCoverage]
public class JwtProvider : IJwtProvider
{
	private readonly JwtOptions _options;

	public JwtProvider(IOptions<JwtOptions> options)
	{
		_options = options.Value;
	}

	public string GenerateToken(Client client)
	{
		var claims = new Claim[]
		{
			new(JwtRegisteredClaimNames.Name, client.Name),
			new(JwtRegisteredClaimNames.Email, client.Email)
		};

		var signingCredentials = new SigningCredentials
		(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
			SecurityAlgorithms.HmacSha256Signature
		);

		var token = new JwtSecurityToken
		(
			_options.Issuer,
			_options.Audience,
			claims,
			null,
			DateTime.UtcNow.AddHours(1),
			signingCredentials
		);
		var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
		return tokenString;
	}

	public TokenInfo DecodeToken(string token)
	{
		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(token);

		var tokenInfo = new TokenInfo
		{
			Name = jwtToken.Claims.First(claim => claim.Type == "name").Value,
			Email = jwtToken.Claims.First(claim => claim.Type == "email").Value
		};

		return tokenInfo;
	}
}