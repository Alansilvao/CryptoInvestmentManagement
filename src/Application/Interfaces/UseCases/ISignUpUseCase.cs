using Application.Commons.Interfaces;
using Application.Dtos.Requests.Clients;

namespace Application.Interfaces.UseCases;

public interface ISignUpUseCase : IUseCase<SignUpRequest, Task>
{
}