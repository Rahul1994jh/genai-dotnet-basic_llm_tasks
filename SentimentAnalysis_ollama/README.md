# Sentiment Analysis with Ollama (Local LLM)

## Overview
This project demonstrates sentiment analysis using Ollama running locally with Microsoft.Extensions.AI. It analyzes product reviews and customer feedback completely offline, extracting sentiment, positive/negative aspects, and emotional tone using the Llama 3.2 model on your own machine.

## Basic Concept
Sentiment analysis determines the emotional tone and opinion expressed in text. This implementation uses a locally-hosted LLM to:
- Classify overall sentiment (Positive, Negative, Neutral, Mixed)
- Identify positive and negative aspects
- Analyze emotional tone
- Generate structured JSON output
- Run completely offline with full privacy

This is valuable for:
- Privacy-sensitive customer feedback analysis
- Offline review processing
- Development and testing without API costs
- Brand reputation management without cloud dependencies
- Edge computing scenarios

## Architecture

```
┌─────────────────────────────────────────┐
│         Local Ollama Server             │
│      (http://localhost:11434)           │
│         Running Llama 3.2               │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│         OllamaSharp Client              │
│      (IChatClient interface)            │
└───────────────┬─────────────────────────┘
                │
┌───────────────▼─────────────────────────┐
│     Sentiment Analysis Logic            │
│  - Format analysis prompt               │
│  - Specify JSON output schema           │
│  - Process multiple reviews             │
│  - Parse structured sentiment data      │
└─────────────────────────────────────────┘
```

### Components

1. **Ollama Server**
   - Runs locally on port 11434
   - Hosts Llama 3.2 model
   - No API keys, no cloud, no internet required

2. **Client Initialization**
   - Creates `OllamaApiClient` for localhost
   - Uses Llama 3.2 model
   - Implements unified `IChatClient` interface

3. **Sentiment Analysis Logic**
   - Defines JSON schema for sentiment output
   - Processes product reviews in batch
   - Extracts structured sentiment data
   - Displays results with token usage

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

## Sample Input

```
This laptop is absolutely amazing! The battery life lasts all day, the screen 
is crystal clear, and it's incredibly fast. Best purchase I've made this year. 
Highly recommend to anyone looking for a reliable machine.
```

## Sample Output

```json
{
  "overallSentiment": "Positive",
  "positiveAspects": [
    "Excellent battery life",
    "Crystal clear screen",
    "Very fast performance",
    "Highly reliable"
  ],
  "negativeAspects": [],
  "emotionalTone": "Enthusiastic and highly satisfied"
}

Tokens used: in=157, out=68
```

## Configuration

### No Configuration Files Needed!

This project requires:
- ✅ No `appsettings.json`
- ✅ No user secrets
- ✅ No API keys
- ✅ No internet connection

Just Ollama installed locally!

## Usage

1. **Start Ollama server** (if not already running):
   ```bash
   ollama serve
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **View sentiment analysis** for 5 example product reviews

## Customization

### Use Different Models

Ollama supports various models optimized for different tasks:

```csharp
// Fast and efficient
IChatClient client = new OllamaApiClient("http://localhost:11434", "mistral");

// Optimized for code understanding
IChatClient client = new OllamaApiClient("http://localhost:11434", "codellama");

// Larger, more capable
IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.1");
```

### Analyze Different Text Types

Replace the `productReviews` array with your own data:

```csharp
var productReviews = new[]
{
    "Your customer feedback text...",
    "Your social media comment...",
    "Your survey response...",
};
```

### Modify Sentiment Schema

Customize the JSON output structure:

```csharp
var sentimentAnalysisPrompt =
@"Analyze sentiment and return JSON:
{{
  ""sentiment"": ""Positive/Negative/Neutral"",
  ""confidence"": number (0-1),
  ""keywords"": [""key terms""],
  ""recommendation"": ""actionable insight""
}}";
```

### Adjust Analysis Parameters

```csharp
new ChatOptions
{
    Temperature = 0.1f,      // Low for consistent classification
    MaxOutputTokens = 200,   // Increase for detailed analysis
}
```

## Key Features

- ✅ **Runs Locally**: Complete privacy, no data leaves your machine
- ✅ **No API Keys**: Zero authentication complexity
- ✅ **Zero Cost**: No per-token charges, unlimited usage
- ✅ **Offline Capable**: Works without internet connection
- ✅ **Batch Processing**: Analyze multiple reviews efficiently
- ✅ **Structured Output**: JSON format for easy parsing
- ✅ **Microsoft.Extensions.AI**: Unified interface

## Comparison: Ollama vs GitHub Models

| Feature | Ollama (This Project) | GitHub Models |
|---------|----------------------|---------------|
| **Privacy** | ✅ 100% local | ⚠️ Cloud-based |
| **Cost** | ✅ Free forever | ⚠️ Token pricing |
| **Internet** | ✅ Not required | ❌ Required |
| **Setup** | ⚠️ Download models | ✅ Just API key |
| **Speed** | ⚠️ Hardware dependent | ✅ Fast cloud GPUs |
| **Model Options** | ⚠️ Limited to Ollama models | ✅ GPT-4o, GPT-4 |
| **Accuracy** | ⚠️ Good (Llama 3.2) | ✅ Excellent (GPT-4o) |

## Example Reviews Analyzed

The application processes 5 different review types:
1. **Highly Positive**: Enthusiastic product praise
2. **Highly Negative**: Strong disappointment and complaints
3. **Mixed**: Both positive and negative aspects
4. **Extremely Positive**: Overwhelming satisfaction
5. **Neutral/Lukewarm**: Indifferent, mediocre experience

## Dependencies

- **Microsoft.Extensions.AI**: Unified AI abstraction layer (v9.0.1-preview.1.24570.5)
- **OllamaSharp**: Ollama client for .NET (v3.1.0)

## Performance Considerations

### Hardware Requirements
- **Minimum**: 8GB RAM, modern CPU
- **Recommended**: 16GB+ RAM, NVIDIA GPU with CUDA
- **Optimal**: 32GB RAM, RTX 3060 or better

### Response Times
- **Llama 3.2 (7B)**: 2-5 seconds per response (CPU)
- **Llama 3.2 (7B)**: 0.5-2 seconds per response (GPU)
- **Smaller models (phi3)**: Faster but less accurate

## Troubleshooting

### Cannot Connect to Ollama

```
Error: Unable to connect to http://localhost:11434
```

**Solutions**:
1. Start Ollama: `ollama serve`
2. Check if port 11434 is available
3. Verify Ollama is installed correctly

### Model Not Found

```
Error: model 'llama3.2' not found
```

**Solution**: Pull the model:
```bash
ollama pull llama3.2
```

### Slow Performance

**Solutions**:
1. Use a smaller model: `ollama pull phi3`
2. Enable GPU acceleration (automatic with NVIDIA CUDA)
3. Reduce `MaxOutputTokens`
4. Close other applications

### Poor Sentiment Accuracy

**Solutions**:
1. Use a larger model: `ollama pull llama3.1:70b`
2. Lower temperature to 0.0 for more consistent results
3. Improve prompt with examples (few-shot learning)
4. Consider cloud models for critical accuracy needs

## Best Practices

1. **Keep Temperature Low**: Use 0.1-0.2 for consistent classification
2. **Clear JSON Schema**: Define exact output structure in prompt
3. **Test Locally First**: Validate with Ollama before deploying to cloud
4. **Batch Process**: Analyze multiple items for efficiency
5. **Monitor Resources**: Track CPU/RAM usage with large models
6. **Model Selection**: Balance speed vs accuracy based on needs

## Switching to Cloud (If Needed)

Want to switch to GitHub Models for better accuracy? Just change the client:

```csharp
// From this (Ollama):
IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2");

// To this (GitHub Models):
IChatClient client = new OpenAIClient(credential, new OpenAIClientOptions
{
    Endpoint = new Uri("https://models.inference.ai.azure.com")
}).GetChatClient("gpt-4o-mini").AsIChatClient();
```

Everything else stays the same! That's the power of `Microsoft.Extensions.AI`. 🚀

## Related Projects

- **[SentimentAnalysis](../SentimentAnalysis/README.md)** - Same task with GitHub Models API
- **[TextCompletion_Ollama](../TextCompletion_Ollama/README.md)** - Basic chat with Ollama
- **[Classification](../Classification/README.md)** - Text categorization
- **[TextExtraction](../TextExtraction/README.md)** - Structured data extraction

## Additional Resources

- [Ollama Official Website](https://ollama.ai)
- [OllamaSharp Documentation](https://github.com/awaescher/OllamaSharp)
- [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/announcing-microsoft-extensions-ai-preview/)
- [Llama 3.2 Release Notes](https://ai.meta.com/blog/llama-3-2-connect-2024-vision-edge-mobile-devices/)

---

**Happy Local AI Development!** 🏠🤖
