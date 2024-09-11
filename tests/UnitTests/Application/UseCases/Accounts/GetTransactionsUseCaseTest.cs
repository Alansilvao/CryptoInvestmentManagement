using Application.Dtos.Requests.Accounts;
using Application.Dtos.Responses.Accounts;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Accounts;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Accounts;

public class GetTransactionsUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IClientsRepository> _clientsRepositoryMock;
	private readonly IGetTransactionsUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public GetTransactionsUseCaseTest()
	{
		_clientsRepositoryMock = new Mock<IClientsRepository>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_useCase = new GetTransactionsUseCase
			(_jwtProviderMock.Object, _clientsRepositoryMock.Object);

		_tokenInfo = new TokenInfo
		{
			Name = "Test",
			Email = "test@test.com"
		};
	}

	[Fact(DisplayName = "Should get transactions extract")]
	public async Task ShouldGetTransactionsExtract()
	{
		var input = new AutoFaker<GetTransactionsExtractRequest>().Generate();
		var expectedResponse = new AutoFaker<GetTransactionsExtractResponse>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_clientsRepositoryMock.Setup(x => x.GetTransactionsExtractAsync(_tokenInfo.Email))
			.ReturnsAsync(expectedResponse);

		var output = await _useCase.ExecuteAsync(input, string.Empty);

		output.Should().BeEquivalentTo(expectedResponse);
	}

	[Fact(DisplayName = "Should throw if repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var input = new AutoFaker<GetTransactionsExtractRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_clientsRepositoryMock.Setup(x => x.GetTransactionsExtractAsync(_tokenInfo.Email))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}

	[Fact(DisplayName = "Should throw if Decode Token throws")]
	public async Task ShouldThrowIfDecodeTokenThrows()
	{
		var input = new AutoFaker<GetTransactionsExtractRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Throws(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}
