using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WikiInsight;
using WikiInsight.Service;

var builder = WebApplication.CreateBuilder(args);
Startup.ConfigureServices(builder);
var app = builder.Build();

// var indexingService = app.Services.GetRequiredService<IndexingService>();
// await indexingService.BuildDocumentIndex(SourceData.LandmarkNames);

app.UseCors("FrontendCors");
