using System.Text.Json.Serialization;

namespace WikiInsight.Models;

public sealed class WikiApiResponse
{
    [JsonPropertyName("query")]
    public WikiQuery? Query { get; set; }
}