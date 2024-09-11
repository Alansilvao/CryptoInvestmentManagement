using Domain.Entities;

namespace Application.Interfaces.Services;

public class TokenInfo
{
	public string Email { get; set; }
	public string Name { get; set; }
}

public interface IJwtProvider
{
	string GenerateToken(Client client);
	TokenInfo DecodeToken(string token);
}