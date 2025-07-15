using System.ComponentModel.DataAnnotations;
using kobi_notify.Models;

namespace kobi_notify.Models
{
    public class CustomerProfileModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ModelName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string DataSourceId { get; set; } = string.Empty;

        public string DataSourceType { get; set; } = string.Empty;

        public string SqlQuery { get; set; } = string.Empty;

        public int RefreshInterval { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; } = false;

        public ICollection<FieldMapping>? FieldMappings { get; set; }
        public ICollection<FallbackRule>? FallbackRules { get; set; }
    }
}