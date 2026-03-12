# Text Completion with Ollama (Local LLM)

## Overview
This project demonstrates text completion using Ollama running locally with Microsoft.Extensions.AI. It shows how to run AI models on your own machine without requiring API keys or internet connectivity, using the Llama 3.2 model.

## Basic Concept
Text completion (chat completion) is the fundamental building block of LLM interactions. This implementation demonstrates:
- Running LLMs locally using Ollama
- Non-streaming and streaming responses
- Basic chat completion patterns
- Temperature and token control
- Zero cloud costs and full privacy

This is valuable for:
- Privacy-sensitive applications
- Offline AI capabilities
- Development and testing without API costs
- Learning AI integration without external dependencies
- Edge computing and IoT scenarios

## Architecture

```
┌─────────────────────────────────────────┐
│         Local Ollama Server             │
│      (http://localhost:11434)           │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│         OllamaSharp Client              │
│      (IChatClient interface)            │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│        Chat Completion Logic            │
│  - Format user prompts                  │
│  - Send to local Llama model            │
│  - Stream responses in real-time        │
│  - Display token usage                  │
└─────────────────────────────────────────┘
```

### Components

1. **Ollama Server Setup**
   - Runs locally on port 11434
   - Hosts Llama 3.2 model
   - No API keys or cloud configuration needed

2. **Client Initialization**
   - Creates `OllamaApiClient` pointing to localhost
   - Specifies model name (llama3.2)
   - Implements `IChatClient` interface

3. **Chat Completion**
   - Supports both streaming and non-streaming modes
   - Configurable temperature and max tokens
   - Real-time response streaming
   - Token usage tracking

## Prerequisites

### Install Ollama

1. **Download Ollama** from [ollama.ai](https://ollama.ai)

2. **Install the Llama 3.2 model**:
   ```bash
   ollama pull llama3.2
   ```

3. **Verify Ollama is running**:
   ```bash
   ollama serve
   ```

The server should start on `http://localhost:11434`

## Sample Output

### Streaming Response

```
=== CHAT COMPLETION ===

user >>> What is the meaning of life?

--- Streaming Response ---

assistant >>> The meaning of life is a profound and timeless question that 
has been debated by philosophers, theologians, and thinkers throughout 
human history. While there's no single definitive answer, many perspectives 
suggest that life's meaning can be found in:

1. Personal growth and self-discovery
2. Building meaningful relationships
3. Contributing to society and helping others
4. Pursuing passions and purpose
5. Finding happiness and contentment

Ultimately, the meaning of life may be something each person defines for 
themselves based on their values, experiences, and beliefs.
```

## Configuration

### No Configuration Files Needed!

Unlike the GitHub Models version, this project requires:
- ✅ No `appsettings.json`
- ✅ No user secrets
- ✅ No API keys
- ✅ No internet connection

Just install Ollama and you're ready to go!

## Usage

1. **Start Ollama server** (if not already running):
   ```bash
   ollama serve
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **View the streaming response** in real-time

## Customization

### Use Different Models

Ollama supports many models. Change the model in `Program.cs`:

```csharp
// Available models: llama3.2, llama3.1, llama2, mistral, phi3, etc.
IChatClient client = new OllamaApiClient("http://localhost:11434", "mistral");
```

**Popular models:**
- `llama3.2` - Latest Llama model (default)
- `llama3.1` - Previous version, still excellent
- `mistral` - Fast and efficient
- `phi3` - Smaller, faster for simple tasks
- `codellama` - Optimized for code generation

### Switch Between Streaming and Non-Streaming

Toggle between response modes by commenting/uncommenting:

```csharp
// Non-streaming: Get complete response at once
var response = await client.GetResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.1f,
    MaxOutputTokens = 300,
});
Console.WriteLine(response);

// Streaming: Get response token-by-token
var responseStream = client.GetStreamingResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.5f,
    MaxOutputTokens = 300,
});

await foreach (var message in responseStream)
{
    Console.Write(message.Text);
}
```

### Adjust Response Parameters

```csharp
new ChatOptions
{
    Temperature = 0.7f,      // 0.0 = deterministic, 1.0 = creative
    MaxOutputTokens = 500,   // Maximum response length
    TopP = 0.9f,            // Nucleus sampling
}
```

## Key Features

- ✅ **Runs Locally**: Complete privacy, no data leaves your machine
- ✅ **No API Keys**: No authentication or tokens needed
- ✅ **Zero Cost**: No per-token charges
- ✅ **Offline Capable**: Works without internet
- ✅ **Streaming Support**: Real-time response generation
- ✅ **Multiple Models**: Easy to switch between Ollama models
- ✅ **Microsoft.Extensions.AI**: Unified interface compatible with other providers

## Comparison: Ollama vs GitHub Models

| Feature | Ollama (Local) | GitHub Models (Cloud) |
|---------|---------------|----------------------|
| **Privacy** | ✅ Complete (runs locally) | ⚠️ Data sent to cloud |
| **Cost** | ✅ Free (one-time setup) | ⚠️ Token-based pricing |
| **Speed** | ⚠️ Depends on hardware | ✅ Fast cloud infrastructure |
| **Internet** | ✅ Not required | ❌ Required |
| **Setup** | ⚠️ Download models (~4GB+) | ✅ Just API key |
| **Model Quality** | ⚠️ Good (Llama 3.2) | ✅ Excellent (GPT-4o) |
| **API Keys** | ✅ None needed | ❌ Required |

## Switching Between Providers

The beauty of `Microsoft.Extensions.AI` is you can easily switch providers:

```csharp
// Ollama (Local)
IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2");

// GitHub Models (Cloud)
IChatClient client = new OpenAIClient(credential, new OpenAIClientOptions
{
    Endpoint = new Uri("https://models.inference.ai.azure.com")
}).GetChatClient("gpt-4o-mini").AsIChatClient();
```

The rest of your code remains unchanged! 🎉

## Dependencies

- **Microsoft.Extensions.AI**: Unified AI abstraction layer
- **OllamaSharp**: Ollama client for .NET

## Troubleshooting

### Ollama Server Not Running

```
Error: Unable to connect to Ollama server
```

**Solution**: Start Ollama server:
```bash
ollama serve
```

### Model Not Found

```
Error: model 'llama3.2' not found
```

**Solution**: Pull the model:
```bash
ollama pull llama3.2
```

### Slow Response

**Solutions**:
- Use a smaller model (phi3)
- Reduce `MaxOutputTokens`
- Check system resources (CPU/RAM)
- Ensure no other heavy applications are running

## Performance Tips

1. **GPU Acceleration**: Ollama automatically uses GPU if available (NVIDIA CUDA)
2. **Model Size**: Smaller models = faster responses
3. **Context Length**: Shorter prompts = faster processing
4. **Hardware**: 16GB+ RAM recommended for larger models

## Related Projects

- **[TextCompletion](../TextCompletion/README.md)** - Same task with GitHub Models API
- **[SentimentAnalysis_ollama](../SentimentAnalysis_ollama/README.md)** - Sentiment analysis with Ollama
- **[Classification](../Classification/README.md)** - Text categorization
- **[Summarization](../Summarization/README.md)** - Text summarization

## Additional Resources

- [Ollama Documentation](https://ollama.ai)
- [OllamaSharp GitHub](https://github.com/awaescher/OllamaSharp)
- [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/announcing-microsoft-extensions-ai-preview/)
- [Llama 3.2 Model Card](https://ollama.ai/library/llama3.2)

---

**Happy Local AI Development!** 🚀
