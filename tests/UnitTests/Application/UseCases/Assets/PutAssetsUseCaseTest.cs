using Application.Dtos.Requests.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Assets;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Assets;

public class PutAssetsUseCaseTest
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IAssetsRepository> _putAssetsRepositoryMock;
    private readonly IPutAssetsUseCase _useCase;
    private readonly TokenInfo _tokenInfo;

    public PutAssetsUseCaseTest()
    {
        _putAssetsRepositoryMock = new Mock<IAssetsRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _useCase = new PutAssetsUseCase(_jwtProviderMock.Object, _putAssetsRepositoryMock.Object);

        _tokenInfo = new TokenInfo
        {
            Name = "Teste",
            Email = "teste@test.com.br"
        };
    }

    [Fact(DisplayName = "Should Changed Assets")]
    public async Task ShouldChangedAssets()
    {
        var input = new AutoFaker<PutAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
        .Returns(_tokenInfo);
        _putAssetsRepositoryMock.Setup(x => x.PutAssetsAsync(input))
        .ReturnsAsync(true);

        var output = await _useCase.ExecuteAsync(input, string.Empty);

        output.Message.Should().Be("Asset changed successfully");
    }

    [Fact(DisplayName = "Should throw if repository throws")]
    public async Task ShouldThrowIfRepositoryThrows()
    {
        var input = new AutoFaker<PutAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Returns(_tokenInfo);

        _putAssetsRepositoryMock.Setup(x => x.PutAssetsAsync(input))
            .ThrowsAsync(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact(DisplayName = "Should throw if Decode Token throws")]
    public async Task ShouldThrowIfDecodeTokenThrows()
    {
        var input = new AutoFaker<PutAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Throws(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }
}
