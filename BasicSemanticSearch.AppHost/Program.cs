var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama")
    .WithDataVolume();

var embedding = ollama.AddModel(name: "embedding", modelName: "all-minilm");


builder.AddProject<Projects.SemanticSearch>("semanticsearch")
    .WithReference(embedding)
    .WaitFor(embedding);

builder.Build().Run();
