using Application.Dtos.Requests.Clients;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Register;
using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Clients;

public class SignUpUseCaseTest
{
	private readonly Mock<IAccountsRepository> _accountRepositoryMock;
	private readonly Mock<IClientsRepository> _clientRepositoryMock;
	private readonly Mock<IPasswordManager> _passwordManagerMock;
	private readonly ISignUpUseCase _useCase;

	public SignUpUseCaseTest()
	{
		_accountRepositoryMock = new Mock<IAccountsRepository>();
		_clientRepositoryMock = new Mock<IClientsRepository>();
		_passwordManagerMock = new Mock<IPasswordManager>();
		_useCase = new SignUpUseCase
		(
			_accountRepositoryMock.Object, _clientRepositoryMock.Object, _passwordManagerMock.Object
		);
	}

	[Fact(DisplayName = "Should sign up successfully")]
	public async Task ShouldSignUpSuccessfully()
	{
		var input = new AutoFaker<SignUpRequest>().Generate();
		var newClient = new Client(input.Name, input.Email, "hashedPassword");

		_clientRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync((Client)null);

		_clientRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Client>()))
			.ReturnsAsync(true);

		_accountRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Domain.Entities.Account>()))
			.ReturnsAsync(true);

		var output = await _useCase.ExecuteAsync(input);

		output.Should().NotBeNull();
	}

	[Fact(DisplayName = "Should throw if email already in use")]
	public async Task ShouldThrowIfEmailAlreadyInUse()
	{
		var input = new AutoFaker<SignUpRequest>().Generate();
		var existingClient = new Client(input.Name, input.Email, "hashedPassword");

		_clientRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync(existingClient);

		Func<Task> act = async () => await _useCase.ExecuteAsync(input);

		await act.Should().ThrowAsync<HttpStatusException>();
	}

	[Fact(DisplayName = "Should throw if failed to create client")]
	public async Task ShouldThrowIfFailedToCreateClient()
	{
		var input = new AutoFaker<SignUpRequest>().Generate();
		var newClient = new Client(input.Name, input.Email, "hashedPassword");

		_clientRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync((Client)null);

		_clientRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Client>()))
			.ReturnsAsync(false);

		Func<Task> act = async () => await _useCase.ExecuteAsync(input);

		await act.Should().ThrowAsync<HttpStatusException>();
	}

	[Fact(DisplayName = "Should throw if failed to create account")]
	public async Task ShouldThrowIfFailedToCreateAccount()
	{
		var input = new AutoFaker<SignUpRequest>().Generate();
		var newClient = new Client(input.Name, input.Email, "hashedPassword");

		_clientRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync((Client)null);

		_clientRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Client>()))
			.ReturnsAsync(true);

		_accountRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Account>()))
			.ReturnsAsync(false);

		Func<Task> act = async () => await _useCase.ExecuteAsync(input);

		await act.Should().ThrowAsync<HttpStatusException>();
	}
}
