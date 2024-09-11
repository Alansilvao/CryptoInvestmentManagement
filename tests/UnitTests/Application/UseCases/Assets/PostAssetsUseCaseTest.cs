using Application.Dtos.Requests.Assets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.UseCases;
using Application.UseCases.Assets;
using AutoBogus;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.UseCases.Assets;

public class PostAssetsUseCaseTest
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<IAssetsRepository> _postAssetsRepositoryMock;
    private readonly IPostAssetsUseCase _useCase;
    private readonly TokenInfo _tokenInfo;

    public PostAssetsUseCaseTest()
    {
        _postAssetsRepositoryMock = new Mock<IAssetsRepository>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _useCase = new PostAssetsUseCase(_jwtProviderMock.Object, 
            _postAssetsRepositoryMock.Object);

        _tokenInfo = new TokenInfo
        {
            Name = "Teste",
            Email = "teste@test.com.br"
        };
    }

    [Fact(DisplayName = "Should Register Assets")]
    public async Task ShouldRegisterAssets()
    {
        var input = new AutoFaker<PostAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
        .Returns(_tokenInfo);
        _postAssetsRepositoryMock.Setup(x => x.PostAssetsAsync(input.Symbol, input.Name, input.AvailableQuantity, input.Price))
        .ReturnsAsync(true);

        var output = await _useCase.ExecuteAsync(input, string.Empty);

        output.Message.Should().Be("Asset registered successfully");
    }

    [Fact(DisplayName = "Should throw if repository throws")]
    public async Task ShouldThrowIfRepositoryThrows()
    {
        var input = new AutoFaker<PostAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Returns(_tokenInfo);

        _postAssetsRepositoryMock.Setup(x => x.PostAssetsAsync(input.Symbol, input.Name, input.AvailableQuantity, input.Price))
            .ThrowsAsync(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }

    [Fact(DisplayName = "Should throw if Decode Token throws")]
    public async Task ShouldThrowIfDecodeTokenThrows()
    {
        var input = new AutoFaker<PostAssetsRequest>().Generate();

        _jwtProviderMock.Setup(x => x.DecodeToken(It.IsAny<string>()))
            .Throws(new Exception("Error"));

        Func<Task> act = async () => await _useCase.ExecuteAsync(input, string.Empty);

        await act.Should().ThrowAsync<Exception>();
    }
}
