# Chat Application - Travel Assistant

## Overview
This project demonstrates building an interactive conversational AI application using GitHub Models API and Microsoft.Extensions.AI. It implements a full-featured travel assistant chatbot with conversation history, streaming responses, and a professional system prompt with guardrails.

## Basic Concept
This is a complete chat application showcasing advanced patterns:
- Multi-turn conversations with history management
- System prompts with role definition and guardrails
- Real-time streaming responses with visual feedback
- User input/output loop
- Graceful error handling
- Exit command support

This is valuable for:
- Building production-ready chatbots
- Customer support applications
- Virtual assistants
- Domain-specific AI advisors
- Interactive Q&A systems
- Learning conversational AI patterns

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
┌───────────────▼─────────────────────────┐
│      Conversational Loop                │
│  - Maintain chat history                │
│  - Stream AI responses                  │
│  - Handle user input                    │
│  - Show thinking animation              │
│  - Implement exit conditions            │
└─────────────────────────────────────────┘
```

### Key Components

1. **Configuration Setup**
   - Loads `appsettings.json` for model and endpoint
   - Retrieves GitHub token from user secrets

2. **Client Initialization**
   - Creates OpenAI client configured for GitHub Models
   - Wraps as `IChatClient` for unified interface

3. **System Prompt**
   - Defines AI personality (friendly travel assistant)
   - Sets conversation guidelines and behavior
   - Implements guardrails for safe, professional responses
   - Specifies information gathering and recommendation patterns

4. **Chat History Management**
   - Maintains full conversation context
   - Includes system, user, and assistant messages
   - Enables multi-turn, context-aware conversations

5. **Streaming Response Handler**
   - Shows "thinking" animation while waiting
   - Streams response token-by-token for real-time feel
   - Builds complete response for history

6. **Input Loop**
   - Reads user input continuously
   - Handles exit commands (exit, quit)
   - Manages conversation flow

## Sample Interaction

```
Travel Assistant Chat (type 'exit' or 'quit' to end)

⠋ Thinking...
Hello! 👋 I'm your friendly travel assistant, here to help you discover 
amazing places and experiences around the world. Whether you're planning 
a weekend getaway or a grand adventure, I'm here to help!

Where would you like to travel, or what kind of trip are you thinking about?

Ask me! 🗺️ 
> I'm thinking about visiting Japan in spring

⠋ Thinking...
Wonderful choice! Japan in spring is absolutely magical, especially during 
cherry blossom season (sakura) which typically peaks in late March to early 
April.

To help create the perfect itinerary for you, I'd love to know more:

1. How many days are you planning to stay?
2. What type of experiences interest you most?
   - Traditional culture (temples, tea ceremonies, gardens)
   - Modern city life (Tokyo, technology, shopping)
   - Nature and outdoor activities
   - Food and culinary experiences
3. What's your approximate budget per day?
4. Is this your first time in Japan?

Ask me! 🗺️ 
> About 10 days, interested in culture and food, budget is moderate

⠋ Thinking...
Perfect! A 10-day cultural and culinary journey in Japan with a moderate 
budget sounds amazing. Here's a suggested itinerary...

[conversation continues...]
```

## Configuration

### appsettings.json
```json
{
  "GitHubModels": {
    "Model": "gpt-4o-mini",
    "Endpoint": "https://models.inference.ai.azure.com"
  }
}
```

### User Secrets
Store your GitHub token securely:
```bash
dotnet user-secrets init
dotnet user-secrets set "GitHubModels:Token" "your-github-token"
```

## Usage

1. **Set up user secrets** with your GitHub token
2. **Run the application**:
   ```bash
   dotnet run
   ```
3. **Chat with the AI** - Ask travel questions
4. **Type 'exit' or 'quit'** to end the conversation

## Customization

### Change the AI Role

Modify the `SystemPrompt` to create different assistants:

```csharp
// Cooking Assistant
const string SystemPrompt = """
    You are a helpful cooking assistant who provides recipes, 
    cooking tips, and meal planning advice...
    """;

// Code Mentor
const string SystemPrompt = """
    You are an experienced software engineer mentoring junior developers.
    Help them debug code, explain concepts, and learn best practices...
    """;

// Fitness Coach
const string SystemPrompt = """
    You are a certified fitness coach helping users achieve their 
    health and wellness goals...
    """;
```

### Adjust Streaming Behavior

Customize the thinking animation:

```csharp
static async Task ShowThinkingAnimationAsync()
{
    // Change animation frames
    var frames = new[] { "●", "○", "◉", "○" };  // Or "⣾⣽⣻⢿⡿⣟⣯⣷"
    
    // Adjust speed
    await Task.Delay(50);  // Faster animation
}
```

### Add Chat History Export

```csharp
// Save conversation to file
await File.WriteAllTextAsync(
    "chat-history.json", 
    JsonSerializer.Serialize(chatHistory)
);
```

### Implement Token Limits

```csharp
// Limit history to last 10 messages to control token usage
if (chatHistory.Count > 11)  // System + 10 messages
{
    chatHistory.RemoveRange(1, chatHistory.Count - 11);
}
```

## Key Features

- ✅ **Multi-turn Conversations**: Full context awareness across messages
- ✅ **System Prompts**: Define AI personality and behavior
- ✅ **Streaming Responses**: Real-time token-by-token output
- ✅ **Visual Feedback**: "Thinking" animation during processing
- ✅ **Conversation History**: Maintains full context
- ✅ **Graceful Exit**: Clean shutdown with exit commands
- ✅ **Error Handling**: Recovers from failed requests
- ✅ **Guardrails**: Professional, safe responses

## Advanced Patterns

### System Prompt Best Practices

The travel assistant demonstrates professional system prompt design:

1. **Role Definition**: Clearly defines AI persona
2. **Behavior Guidelines**: Specific instructions for responses
3. **Information Gathering**: Structured question flow
4. **Output Format**: Consistent recommendation structure
5. **Safety Guardrails**: 
   - Prevents harmful content
   - Avoids assumptions
   - Ensures accuracy
   - Maintains professionalism
   - Respects diversity

### Conversation Flow

```
System Message (defines behavior)
    ↓
AI Greeting (initiates conversation)
    ↓
User Input → AI Response (streaming)
    ↓
User Input → AI Response (context-aware)
    ↓
[loop continues...]
    ↓
Exit Command → Graceful Shutdown
```

## Use Cases

### 1. Travel Planning Assistant (Implemented)
- Destination recommendations
- Itinerary planning
- Budget advice
- Cultural guidance

### 2. Customer Support Bot
- Answer product questions
- Troubleshoot issues
- Process returns/refunds
- Escalate complex issues

### 3. Educational Tutor
- Explain concepts
- Provide examples
- Answer student questions
- Track learning progress

### 4. Code Review Assistant
- Review pull requests
- Suggest improvements
- Explain best practices
- Answer technical questions

### 5. Health & Wellness Coach
- Provide fitness advice
- Suggest meal plans
- Track progress
- Motivate users

## Best Practices

1. **Clear System Prompts**: Define role, behavior, and guardrails explicitly
2. **Manage History**: Limit message count to control token usage
3. **Stream Responses**: Better UX with real-time output
4. **Error Recovery**: Handle failed requests gracefully
5. **Exit Strategy**: Provide clear way to end conversation
6. **Visual Feedback**: Show processing state to user
7. **Context Awareness**: Include full history for coherent conversations

## Dependencies

- **Microsoft.Extensions.AI**: Unified AI abstraction layer
- **Microsoft.Extensions.AI.OpenAI**: OpenAI integration
- **Azure.AI.Inference**: GitHub Models API client
- **Microsoft.Extensions.Configuration**: Configuration management

## Token Usage Considerations

Chat applications consume tokens for:
- **System prompt**: ~300 tokens (sent with every request)
- **Conversation history**: 50-200 tokens per message pair
- **Response**: 100-500 tokens per AI message

**Tips to optimize:**
- Limit history to last N messages
- Use shorter system prompts for simple tasks
- Choose efficient models (gpt-4o-mini vs gpt-4o)
- Implement conversation summarization for long chats

## Troubleshooting

### Token Not Found
```
GitHub token not found in user secrets.
```
**Solution**: Configure user secrets:
```bash
dotnet user-secrets set "GitHubModels:Token" "your-token"
```

### Model Not Responding
**Check**:
1. Internet connection
2. GitHub token is valid
3. Model name is correct in `appsettings.json`

### History Too Long Error
```
Error: Maximum context length exceeded
```
**Solution**: Implement history management:
```csharp
// Keep only last 10 messages
if (chatHistory.Count > 11)
    chatHistory.RemoveRange(1, chatHistory.Count - 11);
```

### Streaming Not Working
**Solution**: Ensure you're awaiting the async enumerable:
```csharp
await foreach (var message in chatClient.GetStreamingResponseAsync(chatHistory))
{
    Console.Write(message.Text);
}
```

## Extending the Application

### Add Voice Input/Output
```csharp
// Use System.Speech or Azure Speech Services
var speechRecognizer = new SpeechRecognizer();
string userInput = await speechRecognizer.RecognizeAsync();
```

### Implement Chat Memory
```csharp
// Store conversations in database
await dbContext.ChatSessions.AddAsync(new ChatSession
{
    UserId = userId,
    Messages = chatHistory,
    Timestamp = DateTime.UtcNow
});
```

### Add Function Calling
```csharp
// Let AI call functions (book flights, check weather, etc.)
var tools = new List<AITool>
{
    AIFunctionFactory.Create(BookFlight),
    AIFunctionFactory.Create(GetWeather)
};
```

### Multi-user Support
```csharp
// Maintain separate history per user
var userSessions = new Dictionary<string, List<ChatMessage>>();
var history = userSessions.GetOrAdd(userId, _ => CreateNewSession());
```

## Related Projects

- **[TextCompletion](../TextCompletion/README.md)** - Basic single-turn completions
- **[Classification](../Classification/README.md)** - Text categorization
- **[SentimentAnalysis](../SentimentAnalysis/README.md)** - Sentiment extraction
- **[Summarization](../Summarization/README.md)** - Text summarization

## Additional Resources

- [GitHub Models Documentation](https://docs.github.com/en/github-models)
- [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/announcing-microsoft-extensions-ai-preview/)
- [Building Chatbots Best Practices](https://learn.microsoft.com/en-us/azure/ai-services/openai/how-to/chatgpt)
- [System Prompt Engineering](https://platform.openai.com/docs/guides/prompt-engineering)

---

**Happy Building!** 🤖💬
