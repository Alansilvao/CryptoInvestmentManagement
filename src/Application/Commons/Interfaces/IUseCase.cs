namespace Application.Commons.Interfaces;

public interface IUseCase<in TRequest, TResponse>
{
	Task<TResponse> ExecuteAsync
		(TRequest request, CancellationToken cancellationToken = default);
}