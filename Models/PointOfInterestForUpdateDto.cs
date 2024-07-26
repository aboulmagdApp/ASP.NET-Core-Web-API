using System.ComponentModel.DataAnnotations;

namespace cityInfo.Api.Models
{
    public class PointOfInterestForUpdateDto
    {
        [Required(ErrorMessage = "You Shoud Require Name!")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
