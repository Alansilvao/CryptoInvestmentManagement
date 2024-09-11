using Application.Commons.Interfaces;
using Application.Dtos.Responses.Assets;

namespace Application.Interfaces.UseCases;

public interface IGetAssetBySymbolUseCase : IUseCase<string, GetAssetBySymbolResponse>
{
}