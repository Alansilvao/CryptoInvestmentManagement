using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Requests.Accounts;

[ExcludeFromCodeCoverage]
public class WithdrawRequest
{
	public decimal Value { get; set; }
}