using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Investments;

[ExcludeFromCodeCoverage]
public class BuyAssetResponse
{
	public string Symbol { get; set; }
	public int Quantity { get; set; }
	public decimal UnitPrice { get; set; }
	public decimal TotalPrice { get; set; }

	public BuyAssetResponse(string symbol, int quantity, decimal unitPrice, decimal totalPrice)
	{
		Symbol = symbol;
		Quantity = quantity;
		UnitPrice = unitPrice;
		TotalPrice = totalPrice;
	}
}
