using kobi_notify.Data;
using kobi_notify.DTOs;
using kobi_notify.Models;
using kobi_notify.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace kobi_notify.Services.Implementation
{
    public class DataSourceService : IDataSourceService
    {
        private readonly KobiDbContext _context;

        public DataSourceService(KobiDbContext context)
        {
            _context = context;
        }

        public async Task<string> SaveDataSourceAsync(DataSourceDto dto)
        {
            var dataSource = new DataSource
            {
                DataSourceName = dto.DataSourceName,
                DatabaseType = dto.DatabaseType,
                ConnectionString = dto.ConnectionString,
                SqlQuery = dto.SqlQuery,
                CreatedAt = DateTime.UtcNow
            };

            _context.DataSources.Add(dataSource);
            await _context.SaveChangesAsync();

            return "Data source saved successfully.";
        }

        public async Task<(bool success, object result)> TestConnectionAsync(TestConnectionDto dto)
        {
            try
            {
                using var conn = new SqlConnection(dto.ConnectionString);
                await conn.OpenAsync();
                return (true, "Connection successful.");
            }
            catch (Exception ex)
            {
                return (false, $"Connection failed: {ex.Message}");
            }
        }
    }
}
