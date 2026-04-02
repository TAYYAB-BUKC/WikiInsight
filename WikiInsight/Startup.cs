using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pinecone;
using WikiInsight.Service;

namespace WikiInsight;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var pineconeKey = UtilsService.GetEnvironmentVariable("PINECONE_API_KEY");
        var pineconeIndexName = UtilsService.GetEnvironmentVariable("PINECONE_INDEX_NAME");

        builder.Services.AddSingleton(client =>
            new PineconeClient(pineconeKey).Index(pineconeIndexName));

        builder.Services.AddSingleton<EmbeddingService>();
        builder.Services.AddSingleton<IndexingService>();
        builder.Services.AddSingleton<WikiService>();
    }
}