using Application.Dtos.Requests.Investments;
using Application.Dtos.Responses.Investments;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Investments;
using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Investments;

public class SellAssetUseCaseTest
{
	private readonly Mock<IJwtProvider> _jwtProviderMock;
	private readonly Mock<IClientsRepository> _clientsRepositoryMock;
	private readonly Mock<IAssetsRepository> _assetsRepositoryMock;
	private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;

	private readonly ISellAssetUseCase _useCase;
	private readonly TokenInfo _tokenInfo;

	public SellAssetUseCaseTest()
	{
		_clientsRepositoryMock = new Mock<IClientsRepository>();
		_jwtProviderMock = new Mock<IJwtProvider>();
		_assetsRepositoryMock = new Mock<IAssetsRepository>();
		_portfolioRepositoryMock = new Mock<IPortfolioRepository>();

		_useCase = new SellAssetUseCase
		(
			_jwtProviderMock.Object, _clientsRepositoryMock.Object, _assetsRepositoryMock.Object,
			_portfolioRepositoryMock.Object
		);

		_tokenInfo = new TokenInfo
		{
			Name = "Test",
			Email = "test@test.com"
		};
	}

	[Fact(DisplayName = "Should sell asset")]
	public async Task ShouldSellAsset()
	{
		var request = new AutoFaker<SellAssetRequest>().Generate();
		var expectedAsset = new AutoFaker<Asset>().Generate();
		var clientAccount = new AutoFaker<Account>().Generate();

		var portfolios = new[]
		{
			new Portfolio 
			{ 
				AssetId = expectedAsset.Id,
				Symbol = expectedAsset.Symbol
			}
		};

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(request.AssetSymbol, CancellationToken.None))
			.ReturnsAsync(expectedAsset);

		_clientsRepositoryMock.Setup(x => x.GetClientAccountAsync(_tokenInfo.Email))
			.ReturnsAsync(clientAccount);

        _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(_tokenInfo.Email))
            .ReturnsAsync(new GetPortfolioResponse(portfolios));

        _portfolioRepositoryMock.Setup(x => x.DecrementPortfolioAsync(expectedAsset, request.Quantity, clientAccount.Id))
			.ReturnsAsync(true);

		var output = await _useCase.ExecuteAsync(request, string.Empty);

		output.Quantity.Should().Be(request.Quantity);
		output.Symbol.Should().Be(request.AssetSymbol);
		output.UnitPrice.Should().Be(expectedAsset.Price);
		output.TotalPrice.Should().Be(expectedAsset.Price * request.Quantity);
	}

	[Fact(DisplayName = "Should throw if asset not found")]
	public async Task ShouldThrowIfNotFound()
	{
		var request = new AutoFaker<SellAssetRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(request.AssetSymbol, CancellationToken.None))
			.ReturnsAsync((Asset)null!);

		Func<Task> act = async () => await _useCase.ExecuteAsync(request, string.Empty);

		await act.Should().ThrowAsync<HttpStatusException>()
			.WithMessage("Asset not found");
	}

	[Fact(DisplayName = "Should throw if assets repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var request = new AutoFaker<SellAssetRequest>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(request.AssetSymbol, CancellationToken.None))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(request, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}

	[Fact(DisplayName = "Should throw if portfolio repository throws")]
	public async Task ShouldThrowIfPortfolioRepositorioThrows()
	{
		var request = new AutoFaker<SellAssetRequest>().Generate();
		var expectedAsset = new AutoFaker<Asset>().Generate();
		var clientAccount = new AutoFaker<Account>().Generate();

		_jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
			.Returns(_tokenInfo);

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(request.AssetSymbol, CancellationToken.None))
			.ReturnsAsync(expectedAsset);

		_clientsRepositoryMock.Setup(x => x.GetClientAccountAsync(_tokenInfo.Email))
			.ReturnsAsync(clientAccount);

		_portfolioRepositoryMock.Setup(x => x.DecrementPortfolioAsync(expectedAsset, request.Quantity, clientAccount.Id))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(request, string.Empty);

		await act.Should().ThrowAsync<Exception>();
	}
}
