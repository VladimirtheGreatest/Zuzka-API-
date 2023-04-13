using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuzka.Services.DTO
{
    public class DocumentDto
    {
        public Guid DocumentId { get; set; }
        public string[]? Tags { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int PublishedYear { get; set; }
        public int Rating { get; set; }
        public bool IsBestseller { get; set; }
    }
}
