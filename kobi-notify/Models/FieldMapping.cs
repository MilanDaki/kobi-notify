﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kobi_notify.Models
{
    public class FieldMapping
    {
        [Key]
        public int Id { get; set; }

        public string FieldName { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string MappingPath { get; set; } = string.Empty;

        public int DataModelId { get; set; }

        [ForeignKey("DataModelId")]
        public DataModel? DataModel { get; set; }
    }
}
