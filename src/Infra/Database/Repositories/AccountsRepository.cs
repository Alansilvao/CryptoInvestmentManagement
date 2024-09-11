using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infra.Database.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Repositories;

[ExcludeFromCodeCoverage]
public class AccountsRepository : IAccountsRepository
{
	private readonly ApplicationDbContext _context;

	public AccountsRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> CreateAsync(Account account)
	{
		await _context.Accounts.AddAsync(account);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<decimal> GetAccountBalanceAsync(string clientEmail)
	{
        var client = await _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail);

		if (client is null)
			throw new HttpStatusException(StatusCodes.Status404NotFound, "Account not found");

		return client.Account.Balance;
	}

	public async Task<bool> DepositAsync(string clientEmail, decimal amount)
	{
		var client = await _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail);

		if (client is null)
			throw new HttpStatusException(StatusCodes.Status404NotFound, "Account not found");

		client.Account.Balance += amount;
		client.Account.UpdatedAt = DateTime.Now;
        
		_context.Accounts.Update(client.Account);
		_context.AccountTransactions.Add(new(client.Account.Id, AccountTransactionType.Deposit, amount));

		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> WithdrawAsync(string clientEmail, decimal amount)
	{
        var client = await _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail);

		if (client is null)
			throw new HttpStatusException(StatusCodes.Status404NotFound, "Account not found");

		if (client.Account!.Balance < amount)
			throw new HttpStatusException(StatusCodes.Status400BadRequest, "Insufficient funds");

		client.Account.Balance -= amount;
        client.Account.UpdatedAt = DateTime.Now;

        _context.Accounts.Update(client.Account);
        _context.AccountTransactions.Add(new(client.Account.Id, AccountTransactionType.Withdraw, amount));

		return await _context.SaveChangesAsync() > 0;
	}
}
