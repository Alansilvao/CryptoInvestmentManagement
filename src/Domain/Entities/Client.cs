using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Client
{
    public Client(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }

    public int Id { get; }
    public string Name { get; }
	public string Email { get; }
	public string Password { get; }

    public virtual Account Account { get; set; }    
}