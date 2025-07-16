namespace kobi_notify.DTOs
{
    public class TemplateRequestDto
    {
        public string Mode { get; set; } = "Postcard"; // Postcard or SimpleText
        public string DataModelName { get; set; } = string.Empty;
        public string FontSize { get; set; } = "12pt";
        public string LineHeight { get; set; } = "1.5";
        public string Orientation { get; set; } = "Portrait"; // Portrait or Landscape
        public string PostcardsPerSheet { get; set; } = "1 per Sheet";

        public IFormFile? FrontImage { get; set; } // Optional image upload
        public IFormFile? StampPlaceholder { get; set; } // Optional image upload

        public string HtmlContent { get; set; } = string.Empty; // Editor content
    }
}
