using kobi_notify.DTOs;
using kobi_notify.Models;
using kobi_notify.Models.DTOs;
using kobi_notify.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace KobiNotifyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataModelController : ControllerBase
    {
        private readonly DataModelService _dataModelService;

        public DataModelController(DataModelService dataModelService)
        {
            _dataModelService = dataModelService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DataModelProfileCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ModelName))
                return BadRequest("ModelName is required.");

            var result = await _dataModelService.SaveDataModelAsync(dto);
            return Ok(new { message = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _dataModelService.GetAllProfilesAsync();
            return Ok(profiles);
        }

        [HttpPost("test-query")]
        public async Task<IActionResult> TestSql([FromBody] SqlTestDto model)
        {
            if (string.IsNullOrWhiteSpace(model.SqlQuery))
                return BadRequest("SQL Query is required.");

            var (success, resultOrError) = await _dataModelService.TestSqlQueryAsync(model.SqlQuery);
            if (!success)
                return BadRequest(new { error = resultOrError });

            return Ok(resultOrError);
        }

        [HttpPost("save-field-mappings")]
        public async Task<IActionResult> SaveFieldMappings([FromBody] List<FieldMappingDto> mappings)
        {
            await _dataModelService.SaveFieldMappingsAsync(mappings);
            return Ok(new { message = "Field mappings saved successfully." });
        }

        [HttpPost("fallback-rules")]
        public async Task<IActionResult> SaveFallbackRules(int customerProfileId, [FromBody] List<FallbackRuleDto> rules)
        {
            await _dataModelService.SaveFallbackRulesAsync(customerProfileId, rules);
            return Ok(new { message = "Fallback rules saved successfully." });
        }

        [HttpGet("by-type/{modelType}")]
        public async Task<IActionResult> GetByModelType(string modelType)
        {
            if (string.IsNullOrWhiteSpace(modelType))
                return BadRequest("Model type is required.");

            var models = await _dataModelService.GetModelsByTypeAsync(modelType);
            return Ok(models);
        }
    }
}