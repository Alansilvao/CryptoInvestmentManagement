using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Requests.Accounts;

[ExcludeFromCodeCoverage]
public class DepositRequest
{
	public decimal Value { get; set; }
}