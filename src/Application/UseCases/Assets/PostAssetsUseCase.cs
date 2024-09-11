using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Assets;

public class PostAssetsUseCase : IPostAssetsUseCase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IAssetsRepository _assetsRepository;

    public PostAssetsUseCase(IJwtProvider jwtProvider, IAssetsRepository assetsRepository)
    {
        _jwtProvider = jwtProvider;
        _assetsRepository = assetsRepository;
    }

    public async Task<PostAssetsResponse> ExecuteAsync(PostAssetsRequest request, string token, CancellationToken cancellationToken = default)
    {
        var tokenInfo = _jwtProvider.DecodeToken(token);
        await _assetsRepository.PostAssetsAsync(request.Symbol, request.Name, request.AvailableQuantity, request.Price);
        return new PostAssetsResponse();
    }
}
