using Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities;

[ExcludeFromCodeCoverage]
public class AccountTransaction
{
    public AccountTransaction(int accountId, AccountTransactionType transactionType, decimal value)
	{
		AccountId = accountId;
		TransactionType = transactionType;
		Value = value;
	}

    public int Id { get; }
    public int AccountId { get; }
    public AccountTransactionType TransactionType { get; }
    public decimal Value { get; }
    public DateTime CreatedAt { get; } = DateTime.Now;
}