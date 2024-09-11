using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Responses.Assets
{
    [ExcludeFromCodeCoverage]
    public class DeleteAssetsResponse
    {
        public string Message { get; } = "Asset remove successfully";
    }
}
