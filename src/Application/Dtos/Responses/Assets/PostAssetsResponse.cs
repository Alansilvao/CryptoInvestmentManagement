using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Assets
{
    [ExcludeFromCodeCoverage]
    public class PostAssetsResponse
    {
        public string Message { get; } = "Asset registered successfully";
    }
}
