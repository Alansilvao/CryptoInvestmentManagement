namespace Application.Commons.Interfaces;

public interface IAuthenticatedUseCases<in TRequest, TResponse>
{
	Task<TResponse> ExecuteAsync
		(TRequest request, string token, CancellationToken cancellationToken = default);
}