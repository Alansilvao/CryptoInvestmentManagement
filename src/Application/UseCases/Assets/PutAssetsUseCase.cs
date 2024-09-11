using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Assets;

public class PutAssetsUseCase : IPutAssetsUseCase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IAssetsRepository _assetsRepository;

    public PutAssetsUseCase(IJwtProvider jwtProvider, IAssetsRepository assetsRepository)
    {
        _jwtProvider = jwtProvider;
        _assetsRepository = assetsRepository;
    }

    public async Task<PutAssetsResponse> ExecuteAsync(PutAssetsRequest request, string token, CancellationToken cancellationToken = default)
    {
        var tokenInfo = _jwtProvider.DecodeToken(token);
        await _assetsRepository.PutAssetsAsync(request);
        return new PutAssetsResponse();
    }
}
