using kobi_notify.Data;
using kobi_notify.DTOs;
using kobi_notify.Models;
using kobi_notify.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace KobiNotifyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerProfileController : ControllerBase
{
    private readonly KobiDbContext _context;
    private readonly IConfiguration _configuration;

    public CustomerProfileController(KobiDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // POST: Save or Draft
    [HttpPost("save")]
    public async Task<IActionResult> Save(CustomerProfileModel model)
    {
        if (string.IsNullOrWhiteSpace(model.ModelName)) return BadRequest("ModelName is required.");

        model.CreatedAt = DateTime.UtcNow;
        _context.CustomerProfiles.Add(model);
        await _context.SaveChangesAsync();
        return Ok(new { message = model.IsPublished ? "Published successfully" : "Saved as draft" });
    }

    // GET: All customer profiles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var profiles = await _context.CustomerProfiles.ToListAsync();
        return Ok(profiles);
    }

    // POST: Test SQL Query against PostgreSQL
    [HttpPost("test-query")]
    public async Task<IActionResult> TestSql([FromBody] CustomerProfileModel model)
    {
        if (string.IsNullOrWhiteSpace(model.SqlQuery))
            return BadRequest("SQL Query is required.");

        try
        {
            string? connString = _configuration.GetConnectionString("PostgresConnection");

            if (string.IsNullOrEmpty(connString))
                return BadRequest("PostgreSQL connection string not found.");

            using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(model.SqlQuery, conn);
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
