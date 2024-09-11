using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Investments;

[ExcludeFromCodeCoverage]
public class GetPortfolioResponse
{
	public IEnumerable<Portfolio> Portfolios { get; set; }

	public GetPortfolioResponse(IEnumerable<Portfolio> portfolios)
	{
		Portfolios = portfolios;
	}
}
