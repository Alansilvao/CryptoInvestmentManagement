using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Accounts;

[ExcludeFromCodeCoverage]
public class WithdrawResponse
{
	public string Message { get; } = "Withdraw successfully";
}
