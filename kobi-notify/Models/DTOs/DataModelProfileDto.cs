using kobi_notify.DTOs;

namespace kobi_notify.Models.DTOs
{
    public class DataModelProfileDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DataSourceId { get; set; } = string.Empty;
        public string DataSourceType { get; set; } = string.Empty;
        public string SqlQuery { get; set; } = string.Empty;
        public int RefreshIntervalMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }

        public List<FieldMappingDto>? FieldMappings { get; set; }
        public List<FallbackRuleDto>? FallbackRules { get; set; }
    }
}

