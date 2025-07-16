using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kobi_notify.Models
{
    public class FallbackRule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FieldName { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public string FallbackType { get; set; } = "Constant"; // "Constant" or "Copy Field"

        public string FallbackValue { get; set; } = string.Empty;

        // Foreign Key to CustomerProfile
        public int DataModelId { get; set; }

        [ForeignKey("DataModelId")]
        public DataModel DataModel { get; set; } = null!;
    }
}
