using Application.Dtos.Requests.Accounts;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Accounts;
using AutoBogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Application.UseCases.Accounts;

public class WithdrawUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IAccountsRepository> _accountRepositoryMock;
	private readonly IWithdrawUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public WithdrawUseCaseTest()
	{
		_accountRepositoryMock = new Mock<IAccountsRepository>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_useCase = new WithdrawUseCase(_accountRepositoryMock.Object, _jwtProviderMock.Object);

		_tokenInfo = new TokenInfo
		{
			Name = "Teste",
			Email = "teste@test.com.br"
		};
	}

	[Fact(DisplayName = "Should withdraw money from account")]
	public async Task ShouldWithdrawMoneyFromAccount()
	{
		var input = new AutoFaker<WithdrawRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.WithdrawAsync(_tokenInfo.Email, input.Value))
			.ReturnsAsync(true);

		var output = await _useCase.ExecuteAsync(input, string.Empty);

		output.Message.Should().Be("Withdraw successfully");
	}

	[Fact(DisplayName = "Should throw if repository throws Account Not Found")]
	public async Task ShouldThrowIfRepositoryThrowsAccountNotFound()
	{
		var input = new AutoFaker<WithdrawRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.WithdrawAsync(_tokenInfo.Email, input.Value))
			.ThrowsAsync
				(new HttpStatusException(StatusCodes.Status404NotFound, "Account not found"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<HttpStatusException>();
	}

	[Fact(DisplayName = "Should throw if repository throws Insufficient Funds")]
	public async Task ShouldThrowIfRepositoryThrowsInsufficientFunds()
	{
		var input = new AutoFaker<WithdrawRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.WithdrawAsync(_tokenInfo.Email, input.Value))
			.ThrowsAsync
				(new HttpStatusException(StatusCodes.Status400BadRequest, "Insufficient funds"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<HttpStatusException>();
	}

	[Fact(DisplayName = "Should throw if Decode Token throws")]
	public async Task ShouldThrowIfDecodeTokenThrows()
	{
		var input = new AutoFaker<WithdrawRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Throws(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}
