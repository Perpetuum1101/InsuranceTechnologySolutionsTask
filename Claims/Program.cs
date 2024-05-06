using System.Text.Json.Serialization;
using Infrastructure;
using Application;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Claims API",
        Description = "Marine vessels claims and covers API"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.ResponseBody |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.ResponseBody |
                            HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestBody | 
                            HttpLoggingFields.RequestPath;
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDb");
var mongoDbName = builder.Configuration["MongoDb:DatabaseName"];
builder.Services.AddInfrastructure(sqlConnectionString!, mongoDbConnectionString!, mongoDbName!);
builder.Services.AddApplication();

var app = builder.Build();
app.UseHttpLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Services.MigrateContext();

app.Run();

public partial class Program { }
