using kobi_notify.DTOs;

namespace kobi_notify.Services.Interfaces
{
    public interface IDataSourceService
    {
        Task<string> SaveDataSourceAsync(DataSourceDto dto);
        Task<(bool success, object result)> TestConnectionAsync(TestConnectionDto dto); // Updated
    }
}
