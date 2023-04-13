using System.ComponentModel.DataAnnotations;

namespace Zuzka.Data.Entities
{
    public class Data : SaveConfig
    {
        [Key]
        [Required]
        public Guid DataId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PublishedYear { get; set; }
        public int Rating { get; set; }
        public bool IsBestseller { get; set; }
        public Document? Document { get; set; }
        public Guid? DocumentId { get; set; }
    }
}
