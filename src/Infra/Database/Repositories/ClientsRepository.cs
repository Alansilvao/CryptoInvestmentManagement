using Application.Dtos.Responses.Accounts;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Repositories;

[ExcludeFromCodeCoverage]
public class ClientsRepository : IClientsRepository
{
	private readonly ApplicationDbContext _context;

	public ClientsRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> CreateAsync(Client client)
	{
		await _context.Clients.AddAsync(client);
		await _context.SaveChangesAsync();

		return true;
	}

	public async Task<Client> GetByEmailAsync(string requestEmail)
	{
		var client = await _context.Clients.FirstOrDefaultAsync(client => client.Email == requestEmail);

		return client;
	}

	public async Task<Account> GetClientAccountAsync(string clientEmail)
	{
		var client = await _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail);

		return client.Account;
	}

	public async Task<GetTransactionsExtractResponse> GetTransactionsExtractAsync(string clientEmail)
	{
		var client = await _context.Clients.Include(c => c.Account)
			.FirstOrDefaultAsync(client => client.Email == clientEmail);

		var clientId = client.Account.Id;

		return new GetTransactionsExtractResponse
		{
			AccountTransactions = await _context.AccountTransactions.Where(transaction => transaction.AccountId == clientId).ToListAsync(),
            InvestmentsTransactions = await _context.InvestmentTransactions.Where(transaction => transaction.AccountId == clientId).ToListAsync()
        };
    }
}
