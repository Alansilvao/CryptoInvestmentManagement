using Application.Dtos.Requests.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Accounts;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Accounts;

public class DepositUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IAccountsRepository> _accountRepositoryMock;
	private readonly IDepositUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public DepositUseCaseTest
		()
	{
		_accountRepositoryMock = new Mock<IAccountsRepository>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_useCase = new DepositUseCase(_jwtProviderMock.Object, _accountRepositoryMock.Object);

		_tokenInfo = new TokenInfo
		{
			Name = "Teste",
			Email = "teste@test.com.br"
		};
	}

	[Fact(DisplayName = "Should deposit money in account")]
	public async Task ShouldDepositMoneyInAccount()
	{
		var input = new AutoFaker<DepositRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.DepositAsync(_tokenInfo.Email, input.Value))
			.ReturnsAsync(true);

		var output = await _useCase.ExecuteAsync(input, string.Empty);

		output.Message.Should().Be("Deposit successfully");
	}

	[Fact(DisplayName = "Should throw if repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var input = new AutoFaker<DepositRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_accountRepositoryMock.Setup(x => x.DepositAsync(_tokenInfo.Email, input.Value))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}

	[Fact(DisplayName = "Should throw if Decode Token throws")]
	public async Task ShouldThrowIfDecodeTokenThrows()
	{
		var input = new AutoFaker<DepositRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Throws(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}
