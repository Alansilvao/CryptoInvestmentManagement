using Application.Dtos.Requests.Clients;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Register;

public class SignUpUseCase : ISignUpUseCase
{
	private readonly IAccountsRepository _accountRepository;
	private readonly IClientsRepository _clientRepository;
	private readonly IPasswordManager _passwordManager;

	public SignUpUseCase(IAccountsRepository accountRepository, IClientsRepository clientRepository, IPasswordManager passwordManager)
	{
		_accountRepository = accountRepository;
		_clientRepository = clientRepository;
		_passwordManager = passwordManager;
	}

	public async Task<Task> ExecuteAsync(SignUpRequest request, CancellationToken cancellationToken = default)
	{
		var newClient = new Client
			(request.Name, request.Email, _passwordManager.Hash(request.Password));

		if (await _clientRepository.GetByEmailAsync(newClient.Email) is not null)
			throw new HttpStatusException(StatusCodes.Status409Conflict, "Email already in use");

		if (!await _clientRepository.CreateAsync(newClient))
			throw new HttpStatusException(StatusCodes.Status422UnprocessableEntity, "Failed to create Account");

		var account = new Account(newClient.Id, 0);

		if (!await _accountRepository.CreateAsync(account))
			throw new HttpStatusException(StatusCodes.Status422UnprocessableEntity, "Failed to create Account");

		return Task.CompletedTask;
	}
}