using Microsoft.Extensions.AI;
using OllamaSharp;

namespace WikiInsight.Service;

public class EmbeddingService
{
    public static async Task<float[]> GenerateEmbeddings(string wikipediaPageContent)
    {
        var uri = new Uri("http://localhost:11434");
        var ollama = new OllamaApiClient(uri);
        var vectors = new List<(string Word, float[] Vectors)>();

        ollama.SelectedModel = "qwen3-embedding:0.6b";
        EmbeddingGenerationOptions embeddingOptions = new()
        {
            Dimensions = 512
        };

        var embeddings = await ollama.GenerateVectorAsync(wikipediaPageContent, embeddingOptions);

        return embeddings.ToArray();
    }
}