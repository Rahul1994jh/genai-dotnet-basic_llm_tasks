using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;

#region Configuration Setup
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();
#endregion

#region Authentication
ApiKeyCredential credential = new(configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHub token not found in user secrets. Please configure 'GitHubModels:Token'."));
#endregion

#region Model Configuration
string model = configuration["GitHubModels:Model"] ?? "gpt-4o-mini";

string endpoint = configuration["GitHubModels:Endpoint"] ?? "https://models.inference.ai.azure.com";
#endregion

#region Client Initialization
IChatClient chatClient = new OpenAIClient(credential, new OpenAIClientOptions
{
    Endpoint = new Uri(endpoint)
}).GetChatClient(model)
.AsIChatClient();

#endregion

#region Chat Interaction
const string SystemPrompt = """
    You are a friendly, professional travel assistant who helps people discover amazing places and experiences.
    
    INTRODUCTION:
    Introduce yourself warmly as a travel assistant and ask the user where they would like to travel.
    
    INFORMATION GATHERING:
    Ask thoughtful follow-up questions to understand their preferences:
    1. The destination they are considering
    2. Types of activities they enjoy (adventure, culture, relaxation, etc.)
    3. Their budget range
    4. Duration of their trip
    
    RECOMMENDATIONS:
    Based on the information provided, offer suggestions for:
    - Places to visit
    - Activities and experiences
    - Local cuisine to try
    - Transportation options
    - Best times to visit
    - Cultural customs and etiquette
    - Interesting facts and hidden gems
    
    GUARDRAILS AND CONDUCT:
    - Always be polite, respectful, and professional
    - Be specific and accurate in your recommendations
    - Do not make assumptions about the user's preferences, abilities, or circumstances
    - If you don't have accurate information about a topic, acknowledge it honestly - never make up facts or details
    - Do not use, respond to, or engage with profanity or inappropriate language
    - Treat all users equally regardless of their background, identity, or destination
    - Avoid judgmental statements about destinations, cultures, or travel choices
    - Respect all cultures, religions, and customs in your recommendations
    - If asked about something outside your travel expertise, politely redirect to travel-related topics
    
    CLOSURE:
    At the end of the conversation, ask if they need additional help or have questions about their trip.
    
    Remember: Your goal is to provide helpful, accurate, and respectful travel guidance that enhances the user's travel planning experience.
    """;

List<ChatMessage> chatHistory = [new ChatMessage(ChatRole.System, SystemPrompt)];

Console.WriteLine("Travel Assistant Chat (type 'exit' or 'quit' to end)\n");

// Get initial greeting from AI
try
{
    string greeting = await GetStreamingResponseAsync(chatClient, chatHistory);
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, greeting));
    Console.WriteLine();
}
catch (Exception ex)
{
    Console.WriteLine($"\nError getting initial greeting: {ex.Message}\n");
}

await RunChatLoopAsync(chatClient, chatHistory);

static async Task RunChatLoopAsync(IChatClient chatClient, List<ChatMessage> chatHistory)
{
    while (true)
    {
        // Get user input
        Console.WriteLine("Ask me! 🗺️ ");
        string? userPrompt = Console.ReadLine();

        // Handle exit commands
        if (string.IsNullOrWhiteSpace(userPrompt) || 
            userPrompt.Equals("exit", StringComparison.OrdinalIgnoreCase) || 
            userPrompt.Equals("quit", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Thank you for using Travel Assistant. Safe travels!");
            break;
        }

        // Add user message to history
        chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

        // Get and display AI response
        try
        {
            string assistantResponse = await GetStreamingResponseAsync(chatClient, chatHistory);
            chatHistory.Add(new ChatMessage(ChatRole.Assistant, assistantResponse));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError: {ex.Message}");
            // Remove the last user message if the request failed
            chatHistory.RemoveAt(chatHistory.Count - 1);
        }

        Console.WriteLine();
    }
}

static async Task<string> GetStreamingResponseAsync(IChatClient chatClient, List<ChatMessage> chatHistory)
{
    var responseBuilder = new System.Text.StringBuilder();
    bool firstChunk = true;
    
    // Show thinking animation
    var thinkingTask = ShowThinkingAnimationAsync();
    
    await foreach (var message in chatClient.GetStreamingResponseAsync(chatHistory))
    {
        if (!string.IsNullOrEmpty(message.Text))
        {
            if (firstChunk)
            {
                // Stop thinking animation and clear the line
                thinkingTask.Wait();
                Console.Write("\r" + new string(' ', 20) + "\r");
                firstChunk = false;
            }
            
            responseBuilder.Append(message.Text);
            Console.Write(message.Text);
        }
    }
    
    Console.WriteLine();
    return responseBuilder.ToString();
}

static async Task ShowThinkingAnimationAsync()
{
    var frames = new[] { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };
    int frameIndex = 0;
    
    // Show animation for a brief moment (it will be cleared when first chunk arrives)
    for (int i = 0; i < 5; i++)
    {
        Console.Write($"\r{frames[frameIndex]} Thinking...");
        frameIndex = (frameIndex + 1) % frames.Length;
        await Task.Delay(100);
    }
}
#endregion

