using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

#region Configuration Setup

// Build configuration and load from multiple sources:
// 1. appsettings.json - Application settings (model, endpoint, default prompt)
// 2. User secrets - Secure storage for API token
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

#endregion

#region Authentication

// Retrieve GitHub Models API token from user secrets
// Throws an exception if the token is not configured
ApiKeyCredential credential = new ApiKeyCredential(configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHub token not found in user secrets. Please configure 'GitHubModels:Token'."));

#endregion

#region Model Configuration

// Load model name from configuration (defaults to gpt-4o-mini if not specified)
// Available models: gpt-4o, gpt-4o-mini, o1-preview, o1-mini
string model = configuration["GitHubModels:Model"] ?? "gpt-4o-mini";

// Load endpoint from configuration
string endpoint = configuration["GitHubModels:Endpoint"] ?? "https://models.inference.ai.azure.com";

#endregion

#region Client Initialization

// Create OpenAI client configured for GitHub Models endpoint
// GitHub Models provides access to AI models through Azure's inference API
IChatClient chatClient = new OpenAIClient(credential, new OpenAIClientOptions
{
    Endpoint = new Uri(endpoint)
}).GetChatClient(model)
.AsIChatClient();

#endregion

#region Chat

Console.WriteLine("=== CHAT COMPLETION ===");
Console.WriteLine();

#region User Prompt
// Load prompt from configuration or use default
string prompt = configuration["GitHubModels:DefaultPrompt"] ?? "What is the meaning of life?";
Console.WriteLine($"user >>> {prompt}");
Console.WriteLine();
#endregion

#region Non Streaming Response

// Send a chat completion request and get the response
var response = await chatClient.GetResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.1f, // Controls randomness of the output (0.0 - 1.0)   
    MaxOutputTokens = 300, // Limits the length of the response
});

// Display the model's response
Console.WriteLine($"assistant >>> {response}");
Console.WriteLine($"Tokens used in={response.Usage?.InputTokenCount ?? 0}, out={response.Usage?.OutputTokenCount ?? 0}");
Console.WriteLine();

#endregion

#region Streaming Response

Console.WriteLine("--- Streaming Response ---");
Console.WriteLine();

// Send a chat completion request and get the streaming response
var responseStream = chatClient.GetStreamingResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.5f,
    MaxOutputTokens = 300,
});

// Display the model's response
Console.Write($"assistant >>> ");
await foreach (var message in responseStream)
{
    Console.Write(message.Text);
}
Console.WriteLine();

#endregion

#endregion
