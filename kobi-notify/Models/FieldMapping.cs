using kobi_notify.Models;
namespace kobi_notify.Models
{
    public class FieldMapping
    {
        public int Id { get; set; }

        public string FieldName { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string MappingPath { get; set; } = string.Empty;

        public int CustomerProfileId { get; set; } 

        public DataModel? CustomerProfile { get; set; }

    }
}
