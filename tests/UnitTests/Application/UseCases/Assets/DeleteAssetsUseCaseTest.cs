using Application.Dtos.Requests.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Assets;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Assets;

public class DeleteAssetsUseCaseTest
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IAssetsRepository> _deleteAssetsRepositoryMock;
    private readonly IDeleteAssetsUseCase _useCase;
    private readonly TokenInfo _tokenInfo;

    public DeleteAssetsUseCaseTest()
    {
        _deleteAssetsRepositoryMock = new Mock<IAssetsRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _useCase = new DeleteAssetsUseCase(_jwtProviderMock.Object, _deleteAssetsRepositoryMock.Object);

        _tokenInfo = new TokenInfo
        {
            Name = "Teste",
            Email = "teste@test.com.br"
        };
    }

    [Fact(DisplayName = "Should Remove Assets")]
    public async Task ShouldRemoveAssets()
    {
        var input = new AutoFaker<DeleteAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
        .Returns(_tokenInfo);
        _deleteAssetsRepositoryMock.Setup(x => x.DeleteAssetsAsync(input.Id))
        .ReturnsAsync(true);

        var output = await _useCase.ExecuteAsync(input, string.Empty);

        output.Message.Should().Be("Asset remove successfully");
    }

    [Fact(DisplayName = "Should throw if repository throws")]
    public async Task ShouldThrowIfRepositoryThrows()
    {
        var input = new AutoFaker<DeleteAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Returns(_tokenInfo);

        _deleteAssetsRepositoryMock.Setup(x => x.DeleteAssetsAsync(input.Id))
            .ThrowsAsync(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact(DisplayName = "Should throw if Decode Token throws")]
    public async Task ShouldThrowIfDecodeTokenThrows()
    {
        var input = new AutoFaker<DeleteAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Throws(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }
}
