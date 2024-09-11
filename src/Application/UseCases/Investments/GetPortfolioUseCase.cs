using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;

namespace Application.UseCases.Investments;

public class GetPortfolioUseCase : IGetPortfolioUseCase
{
	private readonly IJwtProvider _jwtProvider;
	private readonly IPortfolioRepository _portfolioRepository;

	public GetPortfolioUseCase(IJwtProvider jwtProvider, IPortfolioRepository portfolioRepository)
	{
		_jwtProvider = jwtProvider;
		_portfolioRepository = portfolioRepository;
	}

	public Task<GetPortfolioResponse> ExecuteAsync
		(GetPortfolioRequest request, string token, CancellationToken cancellationToken = default)
	{
		var tokenInfo = _jwtProvider.DecodeToken(token);
		var portfolios = _portfolioRepository.GetPortfolioAsync(tokenInfo.Email);

		return portfolios;
	}
}
