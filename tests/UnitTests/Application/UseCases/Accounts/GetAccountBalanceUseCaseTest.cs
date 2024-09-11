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

public class GetAccountBalanceUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IAccountsRepository> _accountRepositoryMock;
	private readonly IGetAccountBalanceUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public GetAccountBalanceUseCaseTest()
	{
		_accountRepositoryMock = new Mock<IAccountsRepository>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_useCase = new GetAccountBalanceUseCase
			(_jwtProviderMock.Object, _accountRepositoryMock.Object);

		_tokenInfo = new TokenInfo
		{
			Name = "Test",
			Email = "test@test.com"
		};
	}

	[Fact(DisplayName = "Should get account balance")]
	public async Task ShouldGetAccountBalance()
	{
		var input = new AutoFaker<GetBalanceRequest>().Generate();
		var accountBalance = 1000;

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.GetAccountBalanceAsync(_tokenInfo.Email))
			.ReturnsAsync(accountBalance);

		var output = await _useCase.ExecuteAsync(input, string.Empty);

		output.Balance.Should().Be(accountBalance);
	}

	[Fact(DisplayName = "Should throw if repository throws Account Not Found")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var input = new AutoFaker<GetBalanceRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.GetAccountBalanceAsync(_tokenInfo.Email))
			.ThrowsAsync
				(new HttpStatusException(StatusCodes.Status404NotFound, "Account not found"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}

	[Fact(DisplayName = "Should throw if Decode Token throws")]
	public async Task ShouldThrowIfDecodeTokenThrows()
	{
		var input = new AutoFaker<GetBalanceRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Throws(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}