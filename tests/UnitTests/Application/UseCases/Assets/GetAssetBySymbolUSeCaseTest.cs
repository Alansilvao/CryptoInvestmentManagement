using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.UseCases.Assets;
using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Assets;

public class GetAssetBySymbolUseCaseTest
{
	private readonly Mock<IAssetsRepository> _assetsRepositoryMock;
	private readonly IGetAssetBySymbolUseCase _useCase;

	public GetAssetBySymbolUseCaseTest()
	{
		_assetsRepositoryMock = new Mock<IAssetsRepository>();
		_useCase = new GetAssetBySymbolUseCase(_assetsRepositoryMock.Object);
	}

	[Fact(DisplayName = "Should get asset by symbol")]
	public async Task ShouldGetAssetBySymbol()
	{
		var symbol = "TEST";
		var expectedAsset = new AutoFaker<Asset>().Generate();

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(symbol, CancellationToken.None))
			.ReturnsAsync(expectedAsset);

		var output = await _useCase.ExecuteAsync(symbol);

		output.Asset.Should().BeEquivalentTo(expectedAsset);
	}

	[Fact(DisplayName = "Should throw if asset not found")]
	public async Task ShouldThrowIfNotFound()
	{
		var symbol = "TEST";

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(symbol, CancellationToken.None))
			.ReturnsAsync((Asset?)null);

		Func<Task> act = async () => await _useCase.ExecuteAsync(symbol);

		await act.Should().ThrowAsync<HttpStatusException>()
			.WithMessage("Asset not found");
	}

	[Fact(DisplayName = "Should throw if repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var symbol = "TEST";

		_assetsRepositoryMock.Setup(x => x.GetBySymbolAsync(symbol, CancellationToken.None))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(symbol);

		await act.Should().ThrowAsync<Exception>();
	}
}
