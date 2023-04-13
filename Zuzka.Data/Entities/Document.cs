using System.ComponentModel.DataAnnotations;

namespace Zuzka.Data.Entities
{
    public class Document : SaveConfig
    {
        [Key]
        [Required]
        public Guid DocumentId { get; set; }

        public string[]? Tags { get; set; }

        public Data? Data { get; set; }
    }
}
