using kobi_notify.DTOs;
using kobi_notify.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KobiNotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataSourceController : ControllerBase
    {
        private readonly IDataSourceService _dataSourceService;

        public DataSourceController(IDataSourceService dataSourceService)
        {
            _dataSourceService = dataSourceService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveDataSource([FromBody] DataSourceDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DataSourceName) ||
                string.IsNullOrWhiteSpace(dto.DatabaseType) ||
                string.IsNullOrWhiteSpace(dto.ConnectionString) ||
                string.IsNullOrWhiteSpace(dto.SqlQuery))
            {
                return BadRequest("All fields are required.");
            }

            var result = await _dataSourceService.SaveDataSourceAsync(dto);
            return Ok(new { message = result });
        }

        [HttpPost("test-connection")]
        public async Task<IActionResult> TestConnection([FromBody] TestConnectionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ConnectionString))
                return BadRequest("Connection string is required.");

            var (success, result) = await _dataSourceService.TestConnectionAsync(dto);

            if (!success)
                return BadRequest(new { error = result });

            return Ok(new { message = result });
        }
    }
}
