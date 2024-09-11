using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Accounts;

[ExcludeFromCodeCoverage]
public class DepositResponse
{
	public string Message { get; } = "Deposit successfully";
}
