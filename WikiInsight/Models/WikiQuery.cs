using System.Text.Json.Serialization;

namespace WikiInsight.Models;

public sealed class WikiQuery
{
    [JsonPropertyName("pages")]
    public List<WikiPage> Pages { get; set; } = [];
}