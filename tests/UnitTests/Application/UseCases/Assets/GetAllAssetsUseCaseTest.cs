using Application.Dtos.Requests.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Application.UseCases.Assets;
using AutoBogus;
using Domain.Entities;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Assets;

public class GetAllAssetsUseCaseTest
{
	private readonly Mock<IAssetsRepository> _assetsRepositoryMock;
	private readonly IGetAllAssetsUseCase _useCase;

	public GetAllAssetsUseCaseTest()
	{
		_assetsRepositoryMock = new Mock<IAssetsRepository>();
		_useCase = new GetAllAssetsUseCase(_assetsRepositoryMock.Object);
	}

	[Fact(DisplayName = "Should get all assets")]
	public async Task ShouldGetAllAssets()
	{
		var input = new AutoFaker<GetAllAssetsRequest>().Generate();
		
        var assets = new[]
		{ 
			new Asset("ALPA4", "ALPARGATAS", 2, 9),
            new Asset("ABEV3", "AMBEV S/A", 5, 10),
            new Asset("AMER3", "AMERICANAS", 5, 5)
        };

        _assetsRepositoryMock.Setup(x => x.GetAllAsync(CancellationToken.None))
			.ReturnsAsync(assets);

		var output = await _useCase.ExecuteAsync(input);

		output.Assets.Should().BeEquivalentTo(assets);
    }

	[Fact(DisplayName = "Should throw if repository throws")]
	public async Task ShouldThrowIfRepositoryThrows()
	{
		var input = new AutoFaker<GetAllAssetsRequest>().Generate();

		_assetsRepositoryMock.Setup(x => x.GetAllAsync(CancellationToken.None))
			.ThrowsAsync(new Exception("Error"));

		Func<Task> act = async () => await _useCase.ExecuteAsync(input);

		await act.Should().ThrowAsync<Exception>();
	}
}
