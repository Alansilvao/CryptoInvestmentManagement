using Application.Dtos.Requests.Accounts;
using Application.Dtos.Responses.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Accounts;

public class GetAccountBalanceUseCase : IGetAccountBalanceUseCase
{
	private readonly IJwtProvider _jwtProvider;
	private readonly IAccountsRepository _accountRepository;

	public GetAccountBalanceUseCase(IJwtProvider jwtProvider, IAccountsRepository accountRepository)
	{
		_jwtProvider = jwtProvider;
		_accountRepository = accountRepository;
	}

	public async Task<GetBalanceResponse> ExecuteAsync(GetBalanceRequest request, string token, CancellationToken cancellationToken = default)
	{
		var tokenInfo = _jwtProvider.DecodeToken(token);
		var balance = await _accountRepository.GetAccountBalanceAsync(tokenInfo.Email);

		return new GetBalanceResponse(balance);
	}
}