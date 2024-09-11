using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IAccountsRepository
{
	Task<bool> CreateAsync(Account account);
	Task<decimal> GetAccountBalanceAsync(string clientEmail);
	Task<bool> DepositAsync(string clientEmail, decimal amount);
	Task<bool> WithdrawAsync(string clientEmail, decimal amount);
}