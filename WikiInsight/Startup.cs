using System.Net.Http.Headers;
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

        builder.Services.AddHttpClient("wikiClient", client =>
        {
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("WikiInsight", "v1.0"));
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(Contact: write2tayyabarsalan+wikipedia@gmail.com)"));
        });

        builder.Services.AddSingleton<EmbeddingService>();
        builder.Services.AddSingleton<IndexingService>();
        builder.Services.AddSingleton<WikiService>();
        builder.Services.AddSingleton<DocumentStoreService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("FrontendCors", policy =>
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                );
        });

        builder.Services.AddSingleton<VectorSearchService>();
        builder.Services.AddSingleton<ArticleChunkStoreService>();
        builder.Services.AddSingleton<ArticleSplitterService>();
    }
}