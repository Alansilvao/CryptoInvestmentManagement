using Application.Dtos.Responses.Assets;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Assets;

public class GetAssetBySymbolUseCase : IGetAssetBySymbolUseCase
{
	private readonly IAssetsRepository _assetsRepository;

	public GetAssetBySymbolUseCase(IAssetsRepository assetsRepository)
	{
		_assetsRepository = assetsRepository;
	}

	public async Task<GetAssetBySymbolResponse> ExecuteAsync
		(string request, CancellationToken cancellationToken = default)
	{
		var asset = await _assetsRepository.GetBySymbolAsync(request, cancellationToken);

		if (asset is null)
			throw new HttpStatusException(StatusCodes.Status404NotFound, "Asset not found");

		return new GetAssetBySymbolResponse(asset);
	}
}