using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WikiInsight;
using WikiInsight.Service;

var builder = WebApplication.CreateBuilder(args);
Startup.ConfigureServices(builder);
var app = builder.Build();

// var indexingService = app.Services.GetRequiredService<IndexingService>();
// await indexingService.BuildDocumentIndex(SourceData.LandmarkNames);

app.UseCors("FrontendCors");

// GET / search?query=...
app.MapGet("/search", async (string query, VectorSearchService vectorSearchService) =>
{
    var results = await vectorSearchService.FindTopKArticles(query, 3);
    return Results.Ok(results);
});

app.Run();