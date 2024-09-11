using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Clients;

[ExcludeFromCodeCoverage]
public class SignInResponse
{
	public string Token { get; set; }

	public SignInResponse(string token)
	{
		Token = token;
	}
}
