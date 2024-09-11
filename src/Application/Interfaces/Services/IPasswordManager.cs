namespace Application.Interfaces.Services;

public interface IPasswordManager
{
	string Hash(string password);
	bool Verify(string password, string hash);
}