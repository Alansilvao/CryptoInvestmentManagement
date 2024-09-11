using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Assets
{
    [ExcludeFromCodeCoverage]
    public class PutAssetsResponse
    {
        public string Message { get; } = "Asset changed successfully";
    }
}
