using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Assets;

[ExcludeFromCodeCoverage]
public class GetAssetBySymbolResponse
{
	public Asset? Asset { get; set; }

	public GetAssetBySymbolResponse(Asset? asset)
	{
		Asset = asset;
	}
}
