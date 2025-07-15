namespace kobi_notify.Models.DTOs
{
    public class FallbackRuleDto
    {
        public string FieldName { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public string FallbackType { get; set; } = "Constant";
        public string FallbackValue { get; set; } = string.Empty;
    }

}
