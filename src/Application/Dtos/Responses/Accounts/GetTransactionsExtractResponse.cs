using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Accounts;

[ExcludeFromCodeCoverage]
public class GetTransactionsExtractResponse
{
	public IEnumerable<InvestmentTransaction> InvestmentsTransactions { get; set; } = null!;
	public IEnumerable<AccountTransaction> AccountTransactions { get; set; } = null!;
}
