using Application.Commons.Interfaces;
using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;

namespace Application.Interfaces.UseCases;

public interface IBuyAssetUseCase : IAuthenticatedUseCases<BuyAssetRequest, BuyAssetResponse>
{
}