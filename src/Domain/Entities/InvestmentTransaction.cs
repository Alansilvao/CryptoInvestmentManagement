using Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class InvestmentTransaction
{
    public InvestmentTransaction(int accountId, int assetId, InvestmentTransactionType transactionType, int quantity, decimal price)
    {
        AccountId = accountId;
        AssetId = assetId;
        TransactionType = transactionType;
        Quantity = quantity;
        Price = price;        
    }

    public int Id { get; set; }
    public int AccountId { get; set; }
    public int AssetId { get; }
    public InvestmentTransactionType TransactionType { get; }
    public int Quantity { get; set; }
    public decimal Price { get; }    
    public DateTime CreatedAt { get; } = DateTime.Now;
}