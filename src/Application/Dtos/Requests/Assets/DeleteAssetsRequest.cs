using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Dtos.Requests.Assets
{
    [ExcludeFromCodeCoverage]
    public class DeleteAssetsRequest
    {
        [Required(ErrorMessage = "Asset Id is required")]
        public int Id { get; set; }
    }
}
