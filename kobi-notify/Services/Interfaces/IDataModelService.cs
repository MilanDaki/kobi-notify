using kobi_notify.DTOs;
using kobi_notify.Models.DTOs;

namespace kobi_notify.Services.Interfaces
{
    public interface IDataModelService
    {
        Task<string> SaveDataModelAsync(DataModelProfileCreateDto dto);
        Task<List<DataModelProfileDto>> GetAllDataModelAsync();
        Task<(bool success, object result)> TestSqlQueryAsync(string sqlQuery);
        Task SaveFieldMappingsAsync(List<FieldMappingDto> mappings);
        Task SaveFallbackRulesAsync(int DataModelId, List<FallbackRuleDto> rules);
    }
}
