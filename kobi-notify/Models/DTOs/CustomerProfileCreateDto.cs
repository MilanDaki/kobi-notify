namespace kobi_notify.Models.DTOs
{
    public class CustomerProfileCreateDto
    {
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DataSourceId { get; set; } = string.Empty;
        public string DataSourceType { get; set; } = string.Empty;
        public string SqlQuery { get; set; } = string.Empty;
        public int RefreshIntervalMinutes { get; set; }
        public bool IsPublished { get; set; }
    }

}
