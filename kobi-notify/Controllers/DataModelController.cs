using kobi_notify.Data;
using kobi_notify.DTOs;
using kobi_notify.Models;
using kobi_notify.Models.DTOs;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KobiNotifyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataModelController : ControllerBase
{
    private readonly KobiDbContext _context;
    private readonly IConfiguration _configuration;

    public DataModelController(KobiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // POST: Save or Draft
    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] CustomerProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ModelName))
            return BadRequest("ModelName is required.");

        var model = new DataModel
        {
            ModelName = dto.ModelName,
            Description = dto.Description,
            DataSourceId = dto.DataSourceId,
            DataSourceType = dto.DataSourceType,
            SqlQuery = dto.SqlQuery,
            RefreshIntervalMinutes = dto.RefreshIntervalMinutes,
            CreatedAt = DateTime.UtcNow,
            IsPublished = dto.IsPublished
        };

        _context.CustomerProfiles.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new { message = model.IsPublished ? "Published successfully" : "Saved as draft" });
    }


    // GET: All customer profiles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await _context.CustomerProfiles
            .Include(p => p.FieldMappings)
            .Include(p => p.FallbackRules)
            .ToListAsync();

        var result = profiles.Select(p => new CustomerProfileDto
        {
            Id = p.Id,
            ModelName = p.ModelName,
            Description = p.Description,
            DataSourceId = p.DataSourceId,
            DataSourceType = p.DataSourceType,
            SqlQuery = p.SqlQuery,
            RefreshIntervalMinutes = p.RefreshIntervalMinutes,
            CreatedAt = p.CreatedAt,
            IsPublished = p.IsPublished,
            FieldMappings = p.FieldMappings?.Select(fm => new FieldMappingDto
            {
                FieldName = fm.FieldName,
                Type = fm.Type,
                MappingPath = fm.MappingPath
            }).ToList(),
            FallbackRules = p.FallbackRules?.Select(fr => new FallbackRuleDto
            {
                FieldName = fr.FieldName,
                IsRequired = fr.IsRequired,
                FallbackType = fr.FallbackType,
                FallbackValue = fr.FallbackValue
            }).ToList()
        });

        return Ok(result);
    }



    // POST: Test SQL Query against PostgreSQL
    [HttpPost("test-query")]
    public async Task<IActionResult> TestSql([FromBody] SqlTestDto model)
    {
        if (string.IsNullOrWhiteSpace(model.SqlQuery))
            return BadRequest("SQL Query is required.");

        try
        {
            string? connString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connString))
                return BadRequest("SQL Server connection string not found.");

            using var conn = new SqlConnection(connString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand(model.SqlQuery, conn);
            var reader = await cmd.ExecuteReaderAsync();

            var results = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                results.Add(row);
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }



    [HttpPost("save-field-mappings")]
    public async Task<IActionResult> SaveFieldMappings([FromBody] List<FieldMappingDto> mappings)
    {
        var entities = mappings.Select(m => new FieldMapping
        {
            FieldName = m.FieldName,
            Type = m.Type,
            MappingPath = m.MappingPath,
            CustomerProfileId = m.CustomerProfileId
        }).ToList();

        _context.FieldMappings.AddRange(entities);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Field mappings saved successfully." });
    }



    [HttpPost("fallback-rules")]
    public async Task<IActionResult> SaveFallbackRules(int customerProfileId, [FromBody] List<FallbackRuleDto> rules)
    {
        var fallbackRules = rules.Select(rule => new FallbackRule
        {
            FieldName = rule.FieldName,
            IsRequired = rule.IsRequired,
            FallbackType = rule.FallbackType,
            FallbackValue = rule.FallbackValue,
            CustomerProfileId = customerProfileId
        }).ToList();

        _context.FallbackRules.AddRange(fallbackRules);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Fallback rules saved successfully." });
    }


}
