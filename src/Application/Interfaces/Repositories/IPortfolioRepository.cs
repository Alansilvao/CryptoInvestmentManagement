using Application.Dtos.Responses.Investments;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPortfolioRepository
{
	Task<bool> IncrementPortfolioAsync(Asset asset, int purchasedQuantity, int accountId);
	Task<bool> DecrementPortfolioAsync(Asset asset, int soldQuantity, int accountId);
	Task<GetPortfolioResponse> GetPortfolioAsync(string clientEmail);
}
