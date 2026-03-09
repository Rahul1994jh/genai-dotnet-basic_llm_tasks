# Text Completion with GitHub Models

## Overview
This project demonstrates basic chat completion using GitHub Models API and Microsoft.Extensions.AI. It showcases both non-streaming and streaming response modes, providing a foundation for building AI-powered conversational applications.

## Basic Concept
Chat completion is the core functionality of large language models (LLMs) where the model generates responses based on user prompts. This implementation demonstrates:
- **Non-Streaming Response**: Complete response returned after processing
- **Streaming Response**: Response generated token-by-token in real-time
- **ChatOptions**: Fine-tuning response behavior with parameters
- **Token Usage Tracking**: Monitoring input/output tokens for cost management

This is the foundation for:
- Chatbots and virtual assistants
- Question answering systems
- Content generation
- Interactive AI applications

## Architecture

```
┌─────────────────────────────────────────┐
│         Configuration Layer             │
│  (appsettings.json + User Secrets)      │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│       Authentication Layer              │
│     (ApiKeyCredential with Token)       │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│         GitHub Models Client            │
│   (OpenAI Client → IChatClient)         │
└───────────────┬─────────────────────────┘
                │
        ┌───────┴───────┐
        │               │
┌───────▼─────┐  ┌──────▼──────┐
│Non-Streaming│  │  Streaming  │
│  Response   │  │  Response   │
│ (Complete)  │  │ (Real-time) │
└─────────────┘  └─────────────┘
```

### Components

1. **Configuration Setup**
   - Loads `appsettings.json` for model, endpoint, and default prompt
   - Retrieves GitHub token from user secrets for security

2. **Authentication**
   - Creates `ApiKeyCredential` with GitHub personal access token
   - Validates token exists before proceeding

3. **Model Configuration**
   - Configurable AI model (default: gpt-4o-mini)
   - GitHub Models endpoint: `https://models.inference.ai.azure.com`

4. **Client Initialization**
   - Initializes OpenAI client configured for GitHub Models
   - Wraps as `IChatClient` for Microsoft.Extensions.AI compatibility

5. **Dual Response Modes**
   - **Non-Streaming**: `GetResponseAsync()` returns complete response
   - **Streaming**: `GetStreamingResponseAsync()` yields tokens in real-time

## Setup Instructions

### Prerequisites
- .NET 10 SDK
- GitHub Personal Access Token with access to GitHub Models
- Visual Studio 2022 or VS Code

### Step 1: Install Dependencies
All required NuGet packages are already defined in `TextCompletion.csproj`:
- Microsoft.Extensions.AI (10.3.0)
- Microsoft.Extensions.AI.OpenAI (10.3.0)
- Microsoft.Extensions.Configuration (10.0.3)
- Microsoft.Extensions.Configuration.UserSecrets (6.0.1)
- Microsoft.Extensions.Configuration.Json (10.0.1)

```bash
dotnet restore
```

### Step 2: Configure User Secrets
The project uses User Secrets to securely store your GitHub token:

```bash
cd TextCompletion
dotnet user-secrets set "GitHubModels:Token" "your-github-pat-token-here"
```

**To get a GitHub Personal Access Token:**
1. Go to https://github.com/settings/tokens
2. Click "Generate new token (classic)"
3. Select scopes (minimum: `repo` access)
4. Copy the generated token
5. Use it in the command above

### Step 3: Configure Application Settings
Edit `appsettings.json` to customize model, endpoint, and default prompt:

```json
{
  "GitHubModels": {
    "Model": "gpt-4o-mini",
    "Endpoint": "https://models.inference.ai.azure.com",
    "DefaultPrompt": "What is the meaning of life?"
  }
}
```

**Available Models:**
- `gpt-4o` - Most capable, best quality responses
- `gpt-4o-mini` - Balanced performance and cost (recommended)
- `o1-preview` - Reasoning model for complex problems
- `o1-mini` - Smaller reasoning model

## Running the Example

### Option 1: Using dotnet CLI
```bash
cd TextCompletion
dotnet run
```

### Option 2: Using Visual Studio
1. Open the solution in Visual Studio
2. Set `TextCompletion` as the startup project
3. Press F5 or click Run

### Option 3: Run from solution root
```bash
dotnet run --project TextCompletion
```

## Expected Output

```
=== CHAT COMPLETION ===

user >>> What is the meaning of life?

assistant >>> The meaning of life is a philosophical question that has been debated for centuries. Different perspectives include finding purpose through relationships, personal growth, helping others, pursuing happiness, or connecting with something greater than oneself. Many believe the meaning is something each person must discover for themselves based on their values and experiences.
Tokens used in=12, out=68

--- Streaming Response ---

assistant >>> The meaning of life varies across different philosophical, religious, and cultural perspectives. Some view it as the pursuit of happiness and fulfillment, others see it in serving a higher purpose or deity, while many believe it's about forming meaningful connections and leaving a positive impact on the world. Ultimately, many philosophers suggest that each individual must find their own meaning through their values, experiences, and goals.
```

## How It Works

### 1. Non-Streaming Response
Complete response is generated and returned:

```csharp
var response = await chatClient.GetResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.1f,
    MaxOutputTokens = 300,
});

Console.WriteLine($"assistant >>> {response}");
Console.WriteLine($"Tokens used in={response.Usage?.InputTokenCount}, out={response.Usage?.OutputTokenCount}");
```

**Advantages:**
- Get complete response at once
- Access token usage statistics
- Simpler to implement
- Better for batch processing

### 2. Streaming Response
Response is generated token-by-token:

```csharp
var responseStream = chatClient.GetStreamingResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.5f,
    MaxOutputTokens = 300,
});

await foreach (var message in responseStream)
{
    Console.Write(message.Text);
}
```

**Advantages:**
- Lower perceived latency
- Better user experience
- Real-time feedback
- Can cancel early

### 3. Processing Flow

**Non-Streaming:**
```
User Prompt → AI Model Processing → Complete Response → Display All
```

**Streaming:**
```
User Prompt → AI Model Processing → Token 1 → Token 2 → ... → Token N
                                      ↓        ↓              ↓
                                   Display  Display        Display
```

## ChatOptions Parameters

Both response modes support `ChatOptions` for fine-tuning:

### Temperature
Controls randomness and creativity:
```csharp
Temperature = 0.1f  // More focused, deterministic
Temperature = 0.5f  // Balanced
Temperature = 1.0f  // More creative, random
```

**Guidelines:**
- **0.0-0.3**: Factual questions, code generation, analysis
- **0.4-0.7**: General conversation, explanations
- **0.8-2.0**: Creative writing, brainstorming

### MaxOutputTokens
Limits response length:
```csharp
MaxOutputTokens = 100   // Short responses
MaxOutputTokens = 300   // Medium responses (default)
MaxOutputTokens = 1000  // Long responses
```

### Other Available Options
```csharp
new ChatOptions
{
    Temperature = 0.7f,
    MaxOutputTokens = 500,
    TopP = 0.9f,                    // Nucleus sampling
    FrequencyPenalty = 0.5f,        // Reduce repetition
    PresencePenalty = 0.3f,         // Encourage new topics
    StopSequences = new[] { "\n\n" } // Stop at double newline
}
```

## Use Cases

### 1. Question Answering
```csharp
var prompt = "What is photosynthesis?";
var response = await chatClient.GetResponseAsync(prompt);
```

### 2. Content Generation
```csharp
var prompt = "Write a professional email requesting a meeting";
var response = await chatClient.GetResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.7f,
    MaxOutputTokens = 300
});
```

### 3. Code Explanation
```csharp
var prompt = "Explain this C# code: async Task<string> GetDataAsync()";
var response = await chatClient.GetResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.2f
});
```

### 4. Interactive Chatbot
```csharp
while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();
    
    var stream = chatClient.GetStreamingResponseAsync(userInput);
    Console.Write("Bot: ");
    await foreach (var chunk in stream)
    {
        Console.Write(chunk.Text);
    }
    Console.WriteLine();
}
```

## Customization Guide

### Change Default Prompt
Edit `appsettings.json`:
```json
{
  "GitHubModels": {
    "DefaultPrompt": "Explain quantum computing in simple terms"
  }
}
```

### Switch Models
For more capable responses:
```json
{
  "GitHubModels": {
    "Model": "gpt-4o"
  }
}
```

### Adjust Response Style
Modify temperature in code:
```csharp
// More creative
new ChatOptions { Temperature = 0.8f }

// More focused
new ChatOptions { Temperature = 0.2f }
```

### Add Multi-turn Conversation
Extend to maintain conversation history:
```csharp
var conversationHistory = new List<ChatMessage>
{
    new ChatMessage(ChatRole.System, "You are a helpful assistant"),
    new ChatMessage(ChatRole.User, "Hello"),
    new ChatMessage(ChatRole.Assistant, "Hi! How can I help?"),
    new ChatMessage(ChatRole.User, "What's the weather?")
};

var response = await chatClient.CompleteAsync(conversationHistory);
```

## Token Usage and Cost Management

### Understanding Tokens
- **Input tokens**: Your prompt length
- **Output tokens**: Response length
- Both contribute to API costs

### Example Token Usage
```
user >>> What is the meaning of life?
Tokens used in=12, out=68
```

**Cost Calculation:**
- Input: 12 tokens × $0.00015 = $0.0000018
- Output: 68 tokens × $0.00060 = $0.0000408
- Total: ~$0.000043 per request (gpt-4o-mini pricing)

### Cost Optimization Tips
1. Use `gpt-4o-mini` for general tasks (10x cheaper than gpt-4o)
2. Limit `MaxOutputTokens` to control costs
3. Use lower temperature for shorter responses
4. Cache common responses
5. Monitor token usage in production

## Streaming vs Non-Streaming: When to Use

### Use Non-Streaming When:
- ✅ You need token usage statistics
- ✅ Processing responses programmatically
- ✅ Batch processing multiple requests
- ✅ Implementing rate limiting
- ✅ Response needs post-processing before display

### Use Streaming When:
- ✅ Building interactive chat UIs
- ✅ User experience is priority
- ✅ Long responses (>500 tokens)
- ✅ Real-time applications
- ✅ Mobile or web applications

## Troubleshooting

### Error: "GitHub token not found in user secrets"
**Solution:** Configure user secrets:
```bash
dotnet user-secrets set "GitHubModels:Token" "your-token-here"
```

### Error: "HTTP 401 Unauthorized"
**Solution:** Token invalid or expired. Generate new GitHub PAT.

### Error: "HTTP 400 Bad Request: unknown_model"
**Solution:** Check model name in `appsettings.json` is valid.

### Slow Response Times
**Solution:**
1. Use `gpt-4o-mini` instead of `gpt-4o`
2. Reduce `MaxOutputTokens`
3. Use streaming for better perceived performance

### Incomplete Streaming Response
**Solution:** Ensure you iterate through entire stream:
```csharp
await foreach (var chunk in responseStream)
{
    // Process all chunks
}
```

## Best Practices

1. **Security**: Never hardcode API tokens
2. **Error Handling**: Wrap API calls in try-catch blocks
3. **Timeouts**: Implement request timeouts
4. **Rate Limiting**: Respect API rate limits
5. **Logging**: Log requests for debugging and monitoring
6. **Cost Monitoring**: Track token usage in production
7. **User Experience**: Use streaming for interactive applications
8. **Temperature Control**: Match temperature to use case

## Advanced Features

### System Messages
Guide model behavior:
```csharp
var messages = new[]
{
    new ChatMessage(ChatRole.System, "You are a professional code reviewer"),
    new ChatMessage(ChatRole.User, "Review this C# code: ...")
};
```

### Function Calling
Enable model to call functions:
```csharp
var tools = new[]
{
    new ChatTool
    {
        Name = "get_weather",
        Description = "Get current weather",
        Parameters = // JSON schema
    }
};
```

### Response Formatting
Request specific formats:
```csharp
var prompt = "List 5 programming languages in JSON format";
```

## Related Projects

- **Classification** - Text classification into categories
- **Summarization** - Text summarization with bullet points
- **SentimentAnalysis** - Product review sentiment analysis

## Specification Reference

See `github-models-spec.md` for complete technical specification including:
- Detailed architecture
- Code generation templates
- Configuration options
- Future enhancements

## License
MIT License

## Support
For issues or questions, please refer to:
- Main project documentation
- GitHub Models documentation at https://github.com/marketplace/models
- Microsoft.Extensions.AI documentation
