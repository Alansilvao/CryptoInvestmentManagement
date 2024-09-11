using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class Portfolio
{
	public int Id { get; set; }
	public int AssetId { get; set; }
	public int AccountId { get; set; }
	public string Symbol { get; set; }
	public int Quantity { get; set; }
	public decimal AveragePurchasePrice { get; set; }
	public decimal AcquisitionValue { get; set; }
	public decimal CurrentValue { get; set; }	
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
