// DTOs/CustomerProfileCreateDto.cs

namespace kobi_notify.DTOs
{
    public class DataModelProfileCreateDto
    {
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DataSourceId { get; set; } = string.Empty;
        public string DataSourceType { get; set; } = string.Empty;
        public string SqlQuery { get; set; } = string.Empty;
        public int RefreshIntervalMinutes { get; set; }
        public bool IsPublished { get; set; }

        // 🔧 Add this line
        public string ModelType { get; set; } = "CustomerProfile";
    }
}
