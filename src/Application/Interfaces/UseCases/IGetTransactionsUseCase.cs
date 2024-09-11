using Application.Commons.Interfaces;
using Application.Dtos.Requests.Accounts;
using Application.Dtos.Responses.Accounts;

namespace Application.Interfaces.UseCases;

public interface IGetTransactionsUseCase : IAuthenticatedUseCases<GetTransactionsExtractRequest,
	GetTransactionsExtractResponse>
{
}