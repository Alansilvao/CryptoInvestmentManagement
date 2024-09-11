using Application.Commons.Interfaces;
using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;

namespace Application.Interfaces.UseCases;

public interface
	IGetPortfolioUseCase : IAuthenticatedUseCases<GetPortfolioRequest, GetPortfolioResponse>
{
}
