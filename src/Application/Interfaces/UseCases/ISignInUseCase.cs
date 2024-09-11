using Application.Commons.Interfaces;
using Application.Dtos.Requests.Clients;
using Application.Dtos.Responses.Clients;

namespace Application.Interfaces.UseCases;

public interface ISignInUseCase : IUseCase<SignInRequest, SignInResponse>
{
}