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

#region Sentiment Analysis

Console.WriteLine("=== SENTIMENT ANALYSIS ===");
Console.WriteLine();

var sentimentAnalysisPrompt = 
@"Analyze the sentiment of the following product review.
Provide the analysis as a JSON object with the following structure:
{{
  ""overallSentiment"": ""Positive, Negative, Neutral, or Mixed"",
  ""positiveAspects"": [""list of positive aspects""],
  ""negativeAspects"": [""list of negative aspects""],
  ""emotionalTone"": ""description of emotional tone""
}}
Review: '{0}'
Return only valid JSON, no additional text.";

// Create example product reviews to analyze
var productReviews = new[]
{
    "This laptop is absolutely amazing! The battery life lasts all day, the screen is crystal clear, and it's incredibly fast. Best purchase I've made this year. Highly recommend to anyone looking for a reliable machine.",
    
    "I'm very disappointed with this coffee maker. It broke after just two weeks of use, and the coffee tastes burnt. Customer service was unhelpful and refused to replace it. Complete waste of money.",
    
    "The headphones have great sound quality and are comfortable to wear for long periods. However, the Bluetooth connection drops occasionally, which is annoying. For the price, they're decent but not perfect.",
    
    "Received my order today and I'm blown away! The quality exceeds my expectations, shipping was fast, and it came beautifully packaged. Will definitely be ordering from this company again!",
    
    "It's okay, I guess. Does what it's supposed to do but nothing special. The design is pretty basic and feels a bit cheap. Probably wouldn't buy it again, but it works for now."
};

// Process each product review
foreach (var review in productReviews)
{
    Console.WriteLine($"Review: \"{review}\"");
    Console.WriteLine();

    var formattedPrompt = string.Format(sentimentAnalysisPrompt, review);
    var sentimentResponse = await chatClient.GetResponseAsync(formattedPrompt, new ChatOptions
    {
        Temperature = 0.1f, 
        MaxOutputTokens = 200,
    });

    Console.WriteLine($"Sentiment Analysis:");
    Console.WriteLine(sentimentResponse.Text);
    Console.WriteLine($"Tokens used: in={sentimentResponse.Usage?.InputTokenCount ?? 0}, out={sentimentResponse.Usage?.OutputTokenCount ?? 0}");
    Console.WriteLine(new string('-', 120));
    Console.WriteLine();
}

#endregion
