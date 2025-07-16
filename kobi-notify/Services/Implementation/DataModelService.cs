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

        public async Task<string> SaveDataModelAsync(DataModelProfileCreateDto dto)
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
                IsPublished = dto.IsPublished,
                ModelType = dto.ModelType // Added
            };

            _context.DataModel.Add(model);
            await _context.SaveChangesAsync();
            return model.IsPublished ? "Published successfully" : "Saved as draft";
        }

        public async Task<List<DataModelProfileDto>> GetAllDataModelAsync()
        {
            var profiles = await _context.DataModel
                .Include(p => p.FieldMappings)
                .Include(p => p.FallbackRules)
                .ToListAsync();

            return profiles.Select(p => new DataModelProfileDto
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
                    DataModelId = fm.DataModelId
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

            int DataModelId = mappings.First().DataModelId;

            var exists = await _context.DataModel.AnyAsync(x => x.Id == DataModelId);
            if (!exists)
                throw new Exception($"Customer profile with ID {DataModelId} does not exist.");

            var existing = _context.FieldMappings.Where(f => f.DataModelId == DataModelId);
            _context.FieldMappings.RemoveRange(existing);

            var entities = mappings.Select(m => new FieldMapping
            {
                FieldName = m.FieldName,
                Type = m.Type,
                MappingPath = m.MappingPath,
                DataModelId = m.DataModelId
            }).ToList();

            _context.FieldMappings.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task SaveFallbackRulesAsync(int DataModelId, List<FallbackRuleDto> rules)
        {
            var fallbackRules = rules.Select(rule => new FallbackRule
            {
                FieldName = rule.FieldName,
                IsRequired = rule.IsRequired,
                FallbackType = rule.FallbackType,
                FallbackValue = rule.FallbackValue,
                DataModelId = DataModelId
            }).ToList();

            _context.FallbackRules.AddRange(fallbackRules);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DataModelProfileDto>> GetModelsByTypeAsync(string modelType)
        {
            var profiles = await _context.DataModel
                .Where(x => x.ModelType.ToLower() == modelType.ToLower())
                .Include(p => p.FieldMappings)
                .Include(p => p.FallbackRules)
                .ToListAsync();

            return profiles.Select(p => new DataModelProfileDto
            {
                Id = p.Id,
                ModelName = p.ModelName,
                Description = p.Description,
                DataSourceId = p.DataSourceId,
                DataSourceType = p.DataSourceType,
                SqlQuery = p.SqlQuery,
                RefreshIntervalMinutes = p.RefreshIntervalMinutes,
                CreatedAt = p.CreatedAt,
                IsPublished = p.IsPublished
            }).ToList();
        }
    }
}