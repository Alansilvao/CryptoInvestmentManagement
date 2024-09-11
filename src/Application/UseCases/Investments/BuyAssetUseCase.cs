using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Investments;

public class BuyAssetUseCase : IBuyAssetUseCase
{
	private readonly IJwtProvider _jwtProvider;
	private readonly IClientsRepository _clientsRepository;
	private readonly IAssetsRepository _assetsRepository;
	private readonly IPortfolioRepository _portfolioRepository;

	public BuyAssetUseCase
	(
		IJwtProvider jwtProvider, IClientsRepository clientsRepository,
		IAssetsRepository assetsRepository,
		IPortfolioRepository portfolioRepository
	)
	{
		_jwtProvider = jwtProvider;
		_clientsRepository = clientsRepository;
		_assetsRepository = assetsRepository;
		_portfolioRepository = portfolioRepository;
	}

	public async Task<BuyAssetResponse> ExecuteAsync(BuyAssetRequest request, string token, CancellationToken cancellationToken = default)
	{
		var tokenInfo = _jwtProvider.DecodeToken(token);
		var clientAccount = await _clientsRepository.GetClientAccountAsync(tokenInfo.Email);
		var wantedAsset = await _assetsRepository.GetBySymbolAsync(request.AssetSymbol, cancellationToken);

		if (wantedAsset is null)
			throw new HttpStatusException(StatusCodes.Status404NotFound, "Asset not found");

		var wantedAssetsTotalPrice = wantedAsset.Price * request.Quantity;

		if (clientAccount!.Balance <= 0 || clientAccount.Balance < wantedAssetsTotalPrice)
			throw new HttpStatusException(StatusCodes.Status400BadRequest, "Insufficient Balance");

		await _portfolioRepository.IncrementPortfolioAsync(wantedAsset, request.Quantity, clientAccount.Id);

		return new BuyAssetResponse(wantedAsset.Symbol, request.Quantity, wantedAsset.Price, wantedAssetsTotalPrice);
	}
}
