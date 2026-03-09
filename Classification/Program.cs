using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

#region Configuration Setup

// Build configuration and load from multiple sources:
// 1. appsettings.json - Application settings (model, endpoint)
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

#region Classification

Console.WriteLine("=== TEXT CLASSIFICATION ===");
Console.WriteLine();

// Define classification prompt and examples
var classificationPrompt = @"Classify the text into one category: 
Complaint, Suggestion, Praise, or Other.
Respond with only the category name.
Text: '{0}'";

// Create example texts to classify
var userFeedbacks = new[]
{
    "The app keeps crashing every time I try to upload a photo. This is really frustrating!",
    "It would be great if you could add a dark mode option to the settings.",
    "Your customer service team was incredibly helpful and resolved my issue within minutes. Thank you!",
    "When does the store close on Sundays?",
    "The delivery was two weeks late and the packaging was damaged. Completely unacceptable."
};

// Process each example
foreach (var feedback in userFeedbacks)
{
    Console.WriteLine($"Text: \"{feedback}\"");

    var formattedPrompt = string.Format(classificationPrompt, feedback);
    var classificationResponse = await chatClient.GetResponseAsync(formattedPrompt, new ChatOptions
    {
        Temperature = 0.1f, 
        MaxOutputTokens = 50,
    });

    Console.WriteLine($"Classification: {classificationResponse.Text}");
    Console.WriteLine();
}

#endregion
