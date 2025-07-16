using System.ComponentModel.DataAnnotations;

namespace kobi_notify.Models
{
    public class DataSource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DataSourceName { get; set; } = string.Empty;

        public string DatabaseType { get; set; } = string.Empty;

        public string ConnectionString { get; set; } = string.Empty;

        public string SqlQuery { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
