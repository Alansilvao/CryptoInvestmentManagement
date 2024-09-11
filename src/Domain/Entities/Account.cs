using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Account
{
    public Account(int clientId, decimal balance)
	{
		ClientId = clientId;
		Balance = balance;
	}

    public int Id { get; }
    public int ClientId { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}