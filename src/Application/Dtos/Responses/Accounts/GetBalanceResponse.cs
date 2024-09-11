using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Accounts;

[ExcludeFromCodeCoverage]
public class GetBalanceResponse
{
	public decimal Balance { get; set; }

	public GetBalanceResponse(decimal balance)
	{
		Balance = balance;
	}
}