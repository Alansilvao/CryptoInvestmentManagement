using Application.Commons.Interfaces;
using Application.Dtos.Requests.Assets;
using Application.Dtos.Responses.Assets;

namespace Application.Interfaces.UseCases
{
    public interface IPutAssetsUseCase : IAuthenticatedUseCases<PutAssetsRequest, PutAssetsResponse>
    {
    }

}
