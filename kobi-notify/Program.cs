using kobi_notify.Data;
using kobi_notify.Services.Implementation;
using kobi_notify.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DataModelService>();

builder.Services.AddDbContext<KobiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDataModelService, DataModelService>();
builder.Services.AddScoped<IDataSourceService, DataSourceService>();




var app = builder.Build();

app.Urls.Add("https://localhost:7094");
app.Urls.Add("http://localhost:5214");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
