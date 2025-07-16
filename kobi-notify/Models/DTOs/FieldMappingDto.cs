using kobi_notify.Models;
namespace kobi_notify.DTOs
{
    public class FieldMappingDto
    {
        public string FieldName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string MappingPath { get; set; } = string.Empty;
        public int DataModelId { get; set; }
    }
}
