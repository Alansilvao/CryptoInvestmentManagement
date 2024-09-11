using Application.Dtos.Responses.Investments;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infra.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Repositories;

[ExcludeFromCodeCoverage]
public class PortfolioRepository : IPortfolioRepository
{
	private readonly ApplicationDbContext _context;

	public PortfolioRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> IncrementPortfolioAsync(Asset asset, int purchasedQuantity, int accountId)
	{
		var assetBd = await _context.Assets.FirstOrDefaultAsync(t => t.Id == asset.Id);
		assetBd.AvailableQuantity -= purchasedQuantity;

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
		account.Balance -= purchasedQuantity * asset.Price;

		await _context.InvestmentTransactions.AddAsync(new(accountId, asset.Id, InvestmentTransactionType.Buy, purchasedQuantity, asset.Price));

        var portfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.AccountId == accountId && p.AssetId == asset.Id);

        if (portfolio is null)
			return await InsertPortfolioAsync(asset, purchasedQuantity, accountId);

		return await UpdatePortfolioAsync(portfolio, asset, purchasedQuantity);		
	}

	public async Task<bool> DecrementPortfolioAsync(Asset asset, int soldQuantity, int accountId)
	{
		var assertPortfolio = await _context.Portfolios.FirstOrDefaultAsync(p => p.AccountId == accountId && p.Symbol == asset.Symbol);

		if (assertPortfolio is null)
			return false;

		assertPortfolio.Quantity -= soldQuantity;

        var assetBd = await _context.Assets.FirstOrDefaultAsync(t => t.Id == asset.Id);
        assetBd.AvailableQuantity += soldQuantity;

        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
		account.Balance += soldQuantity * asset.Price;
        
		await _context.InvestmentTransactions.AddAsync(new(accountId, asset.Id, InvestmentTransactionType.Sell, soldQuantity, asset.Price));

		switch (assertPortfolio.Quantity)
		{
			case < 0:
				return false;
			case 0:
				_context.Portfolios.Remove(assertPortfolio);
				return await _context.SaveChangesAsync() > 0;
			default:
				_context.Portfolios.Update(assertPortfolio);
				return await _context.SaveChangesAsync() > 0;
		}
	}

	private async Task<bool> UpdatePortfolioAsync(Portfolio portfolio, Asset asset, int purchasedQuantity)
	{
		portfolio.Quantity += purchasedQuantity;
		portfolio.AcquisitionValue += purchasedQuantity * asset.Price;
		portfolio.UpdatedAt = DateTime.Now;
		portfolio.AveragePurchasePrice = portfolio.AcquisitionValue / portfolio.Quantity;
		portfolio.CurrentValue = portfolio.Quantity * portfolio.AveragePurchasePrice;
		
		_context.Portfolios.Update(portfolio);

		return await _context.SaveChangesAsync() > 0;
	}

	private async Task<bool> InsertPortfolioAsync(Asset asset, int purchasedQuantity, int accountId)
	{
		var portfolio = new Portfolio
		{
			AccountId = accountId,
			AssetId = asset.Id,
			Quantity = purchasedQuantity,
			Symbol = asset.Symbol,
			AveragePurchasePrice = asset.Price,
			CurrentValue = asset.Price,
			AcquisitionValue = asset.Price * purchasedQuantity
		};

		await _context.Portfolios.AddAsync(portfolio);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<GetPortfolioResponse> GetPortfolioAsync(string clientEmail)
	{
		var account = _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail).Result.Account;

		var portfolios = await _context.Portfolios.Where(p => p.AccountId == account.Id).ToListAsync();

		return new GetPortfolioResponse(portfolios);
	}
}
