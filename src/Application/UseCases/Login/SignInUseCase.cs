using Application.Dtos.Requests.Clients;
using Application.Dtos.Responses.Clients;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Login;

public class SignInUseCase : ISignInUseCase
{
	private readonly IClientsRepository _clientsRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly IJwtProvider _jwtProvider;

	public SignInUseCase(IClientsRepository clientsRepository, IPasswordManager passwordManager, IJwtProvider jwtProvider)
	{
		_clientsRepository = clientsRepository;
		_passwordManager = passwordManager;
		_jwtProvider = jwtProvider;
	}

	public async Task<SignInResponse> ExecuteAsync(SignInRequest request, CancellationToken cancellationToken = default)
	{
		var client = await _clientsRepository.GetByEmailAsync(request.Email);

		if (client is null || !IsPasswordValid(request.Password, client.Password))
			throw new HttpStatusException(StatusCodes.Status401Unauthorized, "Invalid credentials");

		var token = _jwtProvider.GenerateToken(client);
		return new SignInResponse(token);
	}

	private bool IsPasswordValid(string password, string hash)
	{
		return _passwordManager.Verify(password, hash);
	}
}