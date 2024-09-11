using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Investments;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Investments;

public class GetPortfolioUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;

	private readonly IGetPortfolioUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public GetPortfolioUseCaseTest()
	{
		_jwtProviderMock = new Mock<IJwtProvider>();
		_portfolioRepositoryMock = new Mock<IPortfolioRepository>();

		_useCase = new GetPortfolioUseCase(_jwtProviderMock.Object, _portfolioRepositoryMock.Object);

		_tokenInfo = new TokenInfo
		{
			Name = "Test",
			Email = "test@test.com"
		};
	}

	[Fact(DisplayName = "Should get portfolio")]
	public async Task ShouldGetPortfolio()
	{
		var request = new GetPortfolioRequest();
		var expectedPortfolio = new AutoFaker<GetPortfolioResponse>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_portfolioRepositoryMock.Setup
				(x => x.GetPortfolioAsync(_tokenInfo.Email))
			.ReturnsAsync(expectedPortfolio);

		var output = await _useCase.ExecuteAsync(request, string.Empty);

		output.Should().BeEquivalentTo(expectedPortfolio);
	}

	[Fact(DisplayName = "Should throw if repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var request = new GetPortfolioRequest();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(_tokenInfo.Email))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(request, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}
