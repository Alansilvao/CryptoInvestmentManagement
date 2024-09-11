using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Assets;

public class GetAllAssetsUseCase : IGetAllAssetsUseCase
{
	private readonly IAssetsRepository _assetsRepository;

	public GetAllAssetsUseCase(IAssetsRepository assetsRepository)
	{
		_assetsRepository = assetsRepository;
	}

	public async Task<GetAllAssetsResponse> ExecuteAsync(GetAllAssetsRequest request, CancellationToken cancellationToken = default)
	{
		var assets = await _assetsRepository.GetAllAsync(cancellationToken);

		return new GetAllAssetsResponse(assets);
	}
}
