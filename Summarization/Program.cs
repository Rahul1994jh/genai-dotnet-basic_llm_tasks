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

#region Summarization

Console.WriteLine("=== TEXT SUMMARIZATION ===");
Console.WriteLine();

var summaryPrompt = @"Summarize the following text as bullet points for easy understanding.
Provide 2-5 bullet points focusing on the main points and key information.
Text: '{0}'";

// Create example texts to summarize
var textsToSummarize = new[]
{
    @"The quarterly earnings report shows that our company exceeded expectations with a 15% increase in revenue compared to last year. 
    This growth was primarily driven by strong sales in the Asia-Pacific region and the successful launch of our new product line. 
    Operating expenses remained stable, and our profit margin improved from 12% to 14%. 
    The board of directors has approved a dividend increase of 5% for shareholders.",

    @"Climate change is accelerating at an unprecedented rate, with global temperatures rising faster than previously predicted. 
    Scientists warn that without immediate action to reduce carbon emissions, we could see catastrophic environmental changes within the next decade. 
    The melting of polar ice caps is contributing to rising sea levels, threatening coastal communities worldwide. 
    Extreme weather events such as hurricanes, droughts, and wildfires are becoming more frequent and intense.",

    @"Artificial intelligence is transforming the healthcare industry by enabling faster and more accurate diagnoses. 
    Machine learning algorithms can analyze medical images to detect diseases like cancer at earlier stages than traditional methods. 
    AI-powered systems are also being used to predict patient outcomes and personalize treatment plans. 
    However, concerns remain about data privacy and the need for human oversight in critical medical decisions."
};

// Process each example
foreach (var text in textsToSummarize)
{
    Console.WriteLine($"Original Text: {System.Text.RegularExpressions.Regex.Replace(text.Trim(), @"\s+", " ")}");
    Console.WriteLine();

    var formattedPrompt = string.Format(summaryPrompt, text);
    var summaryResponse = await chatClient.GetResponseAsync(formattedPrompt, new ChatOptions
    {
        Temperature = 0.1f, 
        MaxOutputTokens = 150,
    });

    Console.WriteLine($"Summary:");
    Console.WriteLine(summaryResponse.Text);
    Console.WriteLine($"Tokens used: in={summaryResponse.Usage?.InputTokenCount ?? 0}, out={summaryResponse.Usage?.OutputTokenCount ?? 0}");
    Console.WriteLine(new string('-', 120));
    Console.WriteLine();
}

#endregion
