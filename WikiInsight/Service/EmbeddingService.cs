using Microsoft.Extensions.AI;
using OllamaSharp;

namespace WikiInsight.Service;

public class EmbeddingService
{
    public async Task<float[]> GenerateEmbeddings(string wikipediaPageContent, string? apiURL = "http://localhost:11434", string? model = "qwen3-embedding:0.6b")
    {
        var uri = new Uri(apiURL);
        var ollama = new OllamaApiClient(uri)
        {
            SelectedModel = model
        };

        EmbeddingGenerationOptions embeddingOptions = new()
        {
            Dimensions = 512
        };

        var embeddings = await ollama.GenerateVectorAsync(wikipediaPageContent, embeddingOptions);

        return embeddings.ToArray();
    }
}