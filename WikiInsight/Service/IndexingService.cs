using System.Collections.Immutable;
using Pinecone;
using WikiInsight.Models;

namespace WikiInsight.Service;

public class IndexingService
{
    private readonly IndexClient indexClient;
    private readonly WikiService wikiService;
    private readonly EmbeddingService embeddingService;
    private readonly DocumentStoreService documentStoreService;
    private readonly ArticleSplitterService articleSplitterService;
    private readonly ArticleChunkStoreService articleChunkStoreService;

    public IndexingService(IndexClient indexClient, WikiService wikiService, EmbeddingService embeddingService, DocumentStoreService documentStoreService, ArticleSplitterService articleSplitterService, ArticleChunkStoreService articleChunkStoreService)
    {
        this.indexClient = indexClient;
        this.wikiService = wikiService;
        this.embeddingService = embeddingService;
        this.documentStoreService = documentStoreService;
        this.articleSplitterService = articleSplitterService;
        this.articleChunkStoreService = articleChunkStoreService;
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

    public async Task BuildFullArticleIndex(string[] pageTitles)
    {
        foreach (var landmark in pageTitles)
        {
            var wikiPage = await wikiService.GetWikipediaPageForTitle(landmark, true);

            var sections = wikiService.SplitIntoSections(wikiPage.Content);

            var chunks = sections.SelectMany(section =>
                            articleSplitterService
                            .Chunks(wikiPage.Title, section.Content, wikiPage.PageUrl, section.Title))
                            .Take(25)
                            .ToImmutableList();

            List<Vector> pineconeVectors = new();
            foreach (ArticleChunk chunk in chunks)
            {
                var stringsToEmbed = $"{chunk.Title} > {chunk.Section}\n\n{chunk.Content}";
                var vectors = await embeddingService.GenerateEmbeddings(stringsToEmbed);
                var pineconeVector = new Vector
                {
                    Id = chunk.Id,
                    Values = vectors,
                    Metadata = new Metadata
                    {
                        {"title", chunk.Title},
                        {"section", chunk.Section},
                        {"chunk_index", chunk.ChunkIndex},
                    }
                };

                pineconeVectors.Add(pineconeVector);
                articleChunkStoreService.SaveArticleChunk(chunk);
            }

            await indexClient.UpsertAsync(new UpsertRequest
            {
                Vectors = pineconeVectors
            });
        }
    }
}