using kobi_notify.Data;
using kobi_notify.DTOs;
using kobi_notify.Models;
using kobi_notify.Models.DTOs;
using kobi_notify.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace kobi_notify.Services.Implementation
{
    public class DataModelService : IDataModelService
    {
        private readonly KobiDbContext _context;
        private readonly IConfiguration _configuration;

        public DataModelService(KobiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> SaveDataModelAsync(CustomerProfileCreateDto dto)
        {
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
            return model.IsPublished ? "Published successfully" : "Saved as draft";
        }



        public async Task<List<CustomerProfileDto>> GetAllProfilesAsync()
        {
            var profiles = await _context.CustomerProfiles
                .Include(p => p.FieldMappings)
                .Include(p => p.FallbackRules)
                .ToListAsync();

            return profiles.Select(p => new CustomerProfileDto
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
                    MappingPath = fm.MappingPath,
                    CustomerProfileId = fm.CustomerProfileId
                }).ToList(),
                FallbackRules = p.FallbackRules?.Select(fr => new FallbackRuleDto
                {
                    FieldName = fr.FieldName,
                    IsRequired = fr.IsRequired,
                    FallbackType = fr.FallbackType,
                    FallbackValue = fr.FallbackValue
                }).ToList()
            }).ToList();
        }

        public async Task<(bool success, object result)> TestSqlQueryAsync(string sqlQuery)
        {
            try
            {
                string? connString = _configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connString))
                    return (false, "SQL Server connection string not found.");

                using var conn = new SqlConnection(connString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(sqlQuery, conn);
                var reader = await cmd.ExecuteReaderAsync();

                var results = new List<Dictionary<string, object>>();

                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                        row[reader.GetName(i)] = reader.GetValue(i);
                    results.Add(row);
                }

                return (true, results);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task SaveFieldMappingsAsync(List<FieldMappingDto> mappings)
        {
            if (mappings == null || mappings.Count == 0)
                return;

            int customerProfileId = mappings.First().CustomerProfileId;

            // 🔒 Ensure the profile exists
            var exists = await _context.CustomerProfiles.AnyAsync(x => x.Id == customerProfileId);
            if (!exists)
                throw new Exception($"Customer profile with ID {customerProfileId} does not exist.");

            // ✅ Optional: Remove previous mappings
            var existing = _context.FieldMappings.Where(f => f.CustomerProfileId == customerProfileId);
            _context.FieldMappings.RemoveRange(existing);

            // 🔁 Add new mappings
            var entities = mappings.Select(m => new FieldMapping
            {
                FieldName = m.FieldName,
                Type = m.Type,
                MappingPath = m.MappingPath,
                CustomerProfileId = m.CustomerProfileId
            }).ToList();

            _context.FieldMappings.AddRange(entities);
            await _context.SaveChangesAsync();
        }



        public async Task SaveFallbackRulesAsync(int customerProfileId, List<FallbackRuleDto> rules)
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
        }
    }
}
