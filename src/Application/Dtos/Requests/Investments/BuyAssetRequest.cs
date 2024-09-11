using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Requests.Investments;

[ExcludeFromCodeCoverage]
public class BuyAssetRequest
{
	[Required(ErrorMessage = "The asset is required")]
	public string AssetSymbol { get; set; }

	[Required(ErrorMessage = "The asset quantity is required")]
	public int Quantity { get; set; }
}
