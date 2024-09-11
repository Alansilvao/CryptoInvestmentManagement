using Application.Commons.Interfaces;
using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;

namespace Application.Interfaces.UseCases
{
    public interface IPostAssetsUseCase : IAuthenticatedUseCases<PostAssetsRequest, PostAssetsResponse>
    {
    }
}
