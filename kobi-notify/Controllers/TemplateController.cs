using kobi_notify.DTOs;
using kobi_notify.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KobiNotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        [HttpPost("generate-pdf")]
        [RequestSizeLimit(10_000_000)] // 10MB max
        public async Task<IActionResult> GeneratePdf([FromForm] TemplateRequestDto dto)
        {
            var (success, message) = await _templateService.GeneratePdfAsync(dto);
            if (!success)
                return BadRequest(new { error = message });

            return Ok(new { message });
        }
    }
}
