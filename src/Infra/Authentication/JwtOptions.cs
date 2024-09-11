using System.Diagnostics.CodeAnalysis;

namespace Infra.Authentication;

[ExcludeFromCodeCoverage]
public class JwtOptions
{
	public string Issuer { get; init; } = null!;
	public string Audience { get; init; } = null!;
	public string SecretKey { get; init; } = null!;
}