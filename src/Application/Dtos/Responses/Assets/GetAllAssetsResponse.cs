using Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Assets;

[ExcludeFromCodeCoverage]
public class GetAllAssetsResponse
{
	public IEnumerable<Asset> Assets { get; set; }

	public GetAllAssetsResponse(IEnumerable<Asset> assets)
	{
		Assets = assets;
	}
}
