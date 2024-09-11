using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Assets;

public class DeleteAssetsUseCase : IDeleteAssetsUseCase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IAssetsRepository _assetsRepository;

    public DeleteAssetsUseCase(IJwtProvider jwtProvider, IAssetsRepository assetsRepository)
    {
        _jwtProvider = jwtProvider;
        _assetsRepository = assetsRepository;
    }

    public async Task<DeleteAssetsResponse> ExecuteAsync(DeleteAssetsRequest request, string token, CancellationToken cancellationToken = default)
    {
        var tokenInfo = _jwtProvider.DecodeToken(token);
        await _assetsRepository.DeleteAssetsAsync(request.Id);
        return new DeleteAssetsResponse();
    }
}
