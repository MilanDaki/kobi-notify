namespace kobi_notify.DTOs
{
    public class DataSourceDto
    {
        public string DataSourceName { get; set; } = string.Empty;
        public string DatabaseType { get; set; } = string.Empty; // "MySQL" or "Postgres"
        public string ConnectionString { get; set; } = string.Empty;
        public string SqlQuery { get; set; } = string.Empty;
    }
}

