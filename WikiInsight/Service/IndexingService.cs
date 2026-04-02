using Pinecone;

namespace WikiInsight.Service;

public class IndexingService
{
    private readonly IndexClient indexClient;
    private readonly WikiService wikiService;
    private readonly EmbeddingService embeddingService;
    private readonly DocumentStoreService documentStoreService;

    public IndexingService(IndexClient indexClient, WikiService wikiService, EmbeddingService embeddingService, DocumentStoreService documentStoreService)
    {
        this.indexClient = indexClient;
        this.wikiService = wikiService;
        this.embeddingService = embeddingService;
        this.documentStoreService = documentStoreService;
    }

    public async Task BuildDocumentIndex(string[] pageTitles)
    {
        foreach (var landmark in pageTitles)
        {
            var wikiPage = await wikiService.GetWikipediaPageForTitle(landmark);
            var vectors = await embeddingService.GenerateEmbeddings(wikiPage.Content);

            var pineconeVector = new Vector
            {
                Id = wikiPage.Id,
                Values = vectors,
                Metadata = new Metadata
                {
                    { "title", wikiPage.Title }
                },
            };

            await indexClient.UpsertAsync(new UpsertRequest
            {
                Vectors = [pineconeVector]
            });

            documentStoreService.SaveArticle(wikiPage);
        }
    }
}