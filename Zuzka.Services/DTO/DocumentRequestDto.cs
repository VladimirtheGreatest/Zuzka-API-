using System.ComponentModel.DataAnnotations;

namespace Zuzka.Services.DTO
{
    public class DocumentRequestDto 
    {
        public Guid? DocumentId { get; set; }

        [Required]
        public string[]? Tags { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Author { get; set; }
        [Required]
        public int PublishedYear { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public bool IsBestseller { get; set; }
    }
}
