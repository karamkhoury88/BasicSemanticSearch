# Semantic Search with OllamaSharp

This repository contains a semantic search application built using .NET 9 and C# 13.0. The project leverages **OllamaSharp** for embedding generation and cosine similarity for ranking search results. It demonstrates how AI-powered embeddings can be used to perform efficient and meaningful searches over a dataset of articles.

## Table of Contents

- [Features](#features)
- [How It Works](#how-it-works)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Future Enhancements](#future-enhancements)
- [Contributing](#contributing)
- [License](#license)

## Features

- **Semantic Search**: Search articles based on the meaning of the query rather than exact keyword matches.
- **OllamaSharp Integration**: Uses OllamaSharp to generate high-quality embeddings for text data.
- **Cosine Similarity**: Ranks search results by computing the cosine similarity between query embeddings and precomputed article embeddings.
- **Embedded JSON Resource**: Articles are loaded from an embedded JSON file for easy deployment.
- **Continuous Querying**: Supports continuous user input for search queries in a console-based interface.

## How It Works

1. **Embedding Generation**:
   - The application uses the `IEmbeddingGenerator` service from OllamaSharp to generate vector embeddings for articles and user queries.
   - Article embeddings are precomputed by combining the title, description, and content of each article.

2. **Semantic Search**:
   - When a user enters a search query, its embedding is generated.
   - The cosine similarity between the query embedding and each article embedding is calculated.
   - The top 3 most similar articles are displayed, ranked by similarity score.

3. **Data Source**:
   - Articles are stored in an embedded JSON file (`Resources/articles.json`) and loaded into memory at runtime.

## Prerequisites

- .NET 9 SDK
- Visual Studio 2022 or later
- Docker Desktop (required to run the `AppHost` project)
- OllamaSharp library (for embedding generation)

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/karamkhoury88/BasicSemanticSearch
cd BasicSemanticSearch
```

### 2. Install Dependencies

Restore the required NuGet packages:
```bash
dotnet restore
```

### 3. Run the AppHost Project

The `AppHost` project is required to initialize the embedding generator service.

 Ensure Docker Desktop is running, then execute the following command:

```bash
dotnet run --project BasicSemanticSearch.AppHost
```

This will start the necessary services for embedding generation.

### 4. Run the Semantic Search Application

In a separate terminal, run the main `SemanticSearch` project:
```bash
dotnet run --project SemanticSearch
```

### 5. Perform a Search

- When prompted, enter a search query.
- The application will display the top 3 matching articles along with their similarity scores.



## Future Enhancements

- Add support for larger datasets by integrating a database.
- Implement a web-based UI for better user interaction.
- Explore additional similarity metrics for ranking results.
- Add unit tests for key components.

## Contributing

Contributions are welcome! If you'd like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Commit your changes and push them to your fork.
4. Submit a pull request with a detailed description of your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---
Happy coding!

