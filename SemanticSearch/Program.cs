using Microsoft.Extensions.AI; // AI-based utilities for embedding generation
using Microsoft.Extensions.DependencyInjection; // Dependency injection framework
using Microsoft.Extensions.Hosting; // Hosting abstraction for app lifecycle management
using SemanticSearch.Models; // Model definitions for semantic search
using System.Numerics.Tensors; // Tensor utilities for vector operations
using System.Reflection; // Reflection for resource extraction
using System.Text.Json; // JSON processing utilities

try
{
    // Get the embedding generator service
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = GetEmbeddedGenerator();

    // Generate embeddings for articles, combining title, description, and content.
    var articleContentEmbedding = await embeddingGenerator.GenerateAndZipAsync(
        GetArticles().Where(source => source != null).Select(x => x.Title + x.Description + x.Content).ToArray(), // Ensures null values are filtered
        options: null, // No specific options for embedding
        cancellationToken: CancellationToken.None // No cancellation behavior

    );

    // Infinite loop to allow continuous user input for search queries
    while (true)
    {
        Console.WriteLine("Enter your search query: ");
        string? userQuery = Console.ReadLine();

        await SearchAsync(userQuery); // Perform search based on user input
    }

    #region Helpers

    // Asynchronous function that searches for articles based on the user query embedding
    async Task SearchAsync(string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm)) // Ensure input is valid
        {
            var userQueryEmbedding = await embeddingGenerator.GenerateEmbeddingAsync(searchTerm); // Generate vector embedding for the search term

            // Compute cosine similarity between search query embedding and article embeddings
            var topMatches = articleContentEmbedding.Select(x => new
            {
                Text = x.Value, // Retrieved article content
                Similiraty = TensorPrimitives.CosineSimilarity(x.Embedding.Vector.Span, userQueryEmbedding.Vector.Span) // Cosine similarity metric
            })
            .OrderByDescending(x => x.Similiraty) // Sort results based on similarity
            .Take(count: 3); // Return top 3 matches

            // Display matching articles and their similarity scores
            foreach (var match in topMatches)
            {
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine($"Similiraty: {match.Similiraty} | Article: {match.Text}");
                Console.WriteLine("--------------------------------------------------------------------------");
            }
        }
    }

    // Function that loads article data from an embedded JSON resource
    List<Article> GetArticles()
    {
        var assembly = Assembly.GetExecutingAssembly(); // Get current assembly reference
        var resourceName = "SemanticSearch.Resources.articles.json"; // JSON resource path

        // Load embedded JSON file as a stream
        using Stream? stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new Exception($"Resource '{resourceName}' not found.");

        using StreamReader reader = new StreamReader(stream);
        string jsonContent = reader.ReadToEnd(); // Read JSON content from resource

        if (string.IsNullOrWhiteSpace(jsonContent)) // Validate non-empty content
        {
            throw new Exception($"Resource '{resourceName}' is empty.");
        }

        // Deserialize JSON into a list of articles
        List<Article> articles = JsonSerializer.Deserialize<List<Article>>(jsonContent)
        ?? throw new Exception($"No articles found in the resource '{resourceName}'");

        return articles;
    }

    // Initializes and returns an embedding generator service from .NET Aspire components
    IEmbeddingGenerator<string, Embedding<float>> GetEmbeddedGenerator()
    {
        var builder = Host.CreateApplicationBuilder(args); // Create application builder

        builder.AddServiceDefaults(); // Add default Aspire services

        // Register AI embedding generator service using OllamaSharp
        builder.AddOllamaSharpEmbeddingGenerator("embedding");

        // Build and initialize application host
        using var host = builder.Build();

        // Retrieve registered embedding generator service
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator = host.Services.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>()
            ?? throw new Exception("No embedded generator registered."); // Ensure service exists

        return embeddingGenerator;
    }

    #endregion
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message); // Catch and display exceptions
}
