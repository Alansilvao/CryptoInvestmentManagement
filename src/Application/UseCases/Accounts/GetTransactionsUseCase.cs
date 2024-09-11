using Application.Dtos.Requests.Accounts;
using Application.Dtos.Responses.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Accounts;

public class GetTransactionsUseCase : IGetTransactionsUseCase
{
	private readonly IJwtProvider _jwtProvider;
	private readonly IClientsRepository _clientsRepository;

	public GetTransactionsUseCase(IJwtProvider jwtProvider, IClientsRepository clientsRepository)
	{
		_jwtProvider = jwtProvider;
		_clientsRepository = clientsRepository;
	}

	public Task<GetTransactionsExtractResponse> ExecuteAsync(GetTransactionsExtractRequest request, string token, CancellationToken cancellationToken = default)
	{
		var tokenInfo = _jwtProvider.DecodeToken(token);
		var transactions = _clientsRepository.GetTransactionsExtractAsync(tokenInfo.Email);

		return transactions;
	}
}