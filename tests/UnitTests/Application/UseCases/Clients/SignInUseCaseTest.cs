using Application.Dtos.Requests.Clients;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Login;
using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Clients;

public class SignInUseCaseTest
{
	private readonly Mock<IClientsRepository> _clientsRepositoryMock;
	private readonly Mock<IPasswordManager> _passwordManagerMock;
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly ISignInUseCase _useCase;

	public SignInUseCaseTest()
	{
		_clientsRepositoryMock = new Mock<IClientsRepository>();
		_passwordManagerMock = new Mock<IPasswordManager>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_useCase = new SignInUseCase(_clientsRepositoryMock.Object, _passwordManagerMock.Object, _jwtProviderMock.Object);
	}

	[Fact(DisplayName = "Should sign in successfully")]
	public async Task ShouldSignInSuccessfully()
	{
		var input = new SignInRequest("teste@gmail.com", "12345678");
		var clientResponse = new Client("Teste", input.Email, input.Password);

		_clientsRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync(clientResponse);

		_passwordManagerMock.Setup(x => x.Verify(input.Password, clientResponse.Password))
			.Returns(true);

		_jwtProviderMock.Setup(x => x.GenerateToken(clientResponse))
			.Returns("token");

		var output = await _useCase.ExecuteAsync(input);

		output.Should().NotBeNull();
		output.Token.Should().NotBeNull();
	}

	[Fact(DisplayName = "Should throw if invalid credentials")]
	public async Task ShouldThrowIfInvalidCredentials()
	{
		var input = new AutoFaker<SignInRequest>().Generate();
		var clientResponse = new Client("Teste", input.Email, input.Password);


		_clientsRepositoryMock.Setup(x => x.GetByEmailAsync(input.Email))
			.ReturnsAsync(clientResponse);

		Func<Task> act = async () => await _useCase.ExecuteAsync(input);

		await act.Should().ThrowAsync<HttpStatusException>();
	}
}
