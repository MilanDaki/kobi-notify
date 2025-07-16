using kobi_notify.DTOs;

namespace kobi_notify.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<(bool success, string message)> GeneratePdfAsync(TemplateRequestDto dto);
    }
}
