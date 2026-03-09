# Sentiment Analysis with GitHub Models

## Overview
This project demonstrates sentiment analysis using GitHub Models API and Microsoft.Extensions.AI. It analyzes product reviews and generates structured JSON output containing sentiment classifications, positive/negative aspects, and emotional tone analysis.

## Basic Concept
Sentiment analysis is an NLP technique that identifies and extracts subjective information from text. This implementation goes beyond simple positive/negative classification to provide:
- **Overall Sentiment**: Positive, Negative, Neutral, or Mixed
- **Positive Aspects**: List of praised features
- **Negative Aspects**: List of criticized features
- **Emotional Tone**: Description of the reviewer's emotional state

This is valuable for:
- Customer feedback analysis
- Product review monitoring
- Brand reputation management
- Market research insights

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
│    Sentiment Analysis Logic             │
│  - Format prompt with review text       │
│  - Request structured JSON analysis     │
│  - Parse sentiment components           │
│  - Display formatted results            │
└─────────────────────────────────────────┘
```

### Components

1. **Configuration Setup**
   - Loads `appsettings.json` for model and endpoint settings
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

5. **Sentiment Analysis Engine**
   - Defines structured JSON prompt template
   - Processes product reviews
   - Returns multi-dimensional sentiment analysis
   - Tracks token usage for cost monitoring

## Setup Instructions

### Prerequisites
- .NET 10 SDK
- GitHub Personal Access Token with access to GitHub Models
- Visual Studio 2022 or VS Code

### Step 1: Install Dependencies
All required NuGet packages are already defined in `SentimentAnalysis.csproj`:
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
cd SentimentAnalysis
dotnet user-secrets set "GitHubModels:Token" "your-github-pat-token-here"
```

**To get a GitHub Personal Access Token:**
1. Go to https://github.com/settings/tokens
2. Click "Generate new token (classic)"
3. Select scopes (minimum: `repo` access)
4. Copy the generated token
5. Use it in the command above

### Step 3: Configure Application Settings (Optional)
Edit `appsettings.json` to customize the model or endpoint:

```json
{
  "GitHubModels": {
    "Model": "gpt-4o-mini",
    "Endpoint": "https://models.inference.ai.azure.com"
  }
}
```

**Available Models:**
- `gpt-4o` - Most capable, better emotional nuance detection
- `gpt-4o-mini` - Balanced performance and cost (recommended)
- `o1-preview` - Reasoning model for deeper analysis
- `o1-mini` - Smaller reasoning model

## Running the Example

### Option 1: Using dotnet CLI
```bash
cd SentimentAnalysis
dotnet run
```

### Option 2: Using Visual Studio
1. Open the solution in Visual Studio
2. Set `SentimentAnalysis` as the startup project
3. Press F5 or click Run

### Option 3: Run from solution root
```bash
dotnet run --project SentimentAnalysis
```

## Expected Output

```
=== SENTIMENT ANALYSIS ===

Review: "This laptop is absolutely amazing! The battery life lasts all day, the screen is crystal clear, and it's incredibly fast. Best purchase I've made this year. Highly recommend to anyone looking for a reliable machine."

Sentiment Analysis:
{
  "overallSentiment": "Positive",
  "positiveAspects": [
    "Excellent battery life lasting all day",
    "Crystal clear screen quality",
    "Fast performance",
    "Reliable machine",
    "Overall satisfaction"
  ],
  "negativeAspects": [],
  "emotionalTone": "Enthusiastic and highly satisfied, with strong recommendation"
}
Tokens used: in=156, out=78
------------------------------------------------------------------------------------------------------------------------

Review: "I'm very disappointed with this coffee maker. It broke after just two weeks of use, and the coffee tastes burnt. Customer service was unhelpful and refused to replace it. Complete waste of money."

Sentiment Analysis:
{
  "overallSentiment": "Negative",
  "positiveAspects": [],
  "negativeAspects": [
    "Product broke after two weeks",
    "Coffee tastes burnt",
    "Unhelpful customer service",
    "No replacement offered",
    "Poor value for money"
  ],
  "emotionalTone": "Disappointed and frustrated, feeling let down by both product and service"
}
Tokens used: in=143, out=85
------------------------------------------------------------------------------------------------------------------------

[Additional examples...]
```

## How It Works

### 1. Sentiment Analysis Prompt
The system uses a structured prompt with explicit JSON schema:
```
Analyze the sentiment of the following product review.
Provide the analysis as a JSON object with the following structure:
{
  "overallSentiment": "Positive, Negative, Neutral, or Mixed",
  "positiveAspects": ["list of positive aspects"],
  "negativeAspects": ["list of negative aspects"],
  "emotionalTone": "description of emotional tone"
}
Review: '{review_text}'
Return only valid JSON, no additional text.
```

### 2. Processing Flow
```
Product Review → Format Prompt → Send to AI Model → Parse JSON → Display Structured Analysis
```

### 3. JSON Output Structure
The response is structured for easy parsing:
- **overallSentiment**: Single classification
- **positiveAspects**: Array of strengths
- **negativeAspects**: Array of weaknesses
- **emotionalTone**: Nuanced emotional description

### 4. ChatOptions Configuration
- **Temperature: 0.1** - Low temperature for consistent, accurate analysis
- **MaxOutputTokens: 200** - Sufficient for comprehensive JSON response

## Use Cases

### 1. E-commerce Platforms
Automatically analyze product reviews to identify:
- Common praise points
- Recurring complaints
- Overall product sentiment
- Customer satisfaction trends

### 2. Customer Support
Categorize support tickets by sentiment to:
- Prioritize urgent negative feedback
- Identify satisfied customers for testimonials
- Route tickets to appropriate teams

### 3. Brand Monitoring
Track brand sentiment across:
- Social media mentions
- App store reviews
- Customer surveys
- Forum discussions

### 4. Market Research
Analyze customer opinions to:
- Guide product development
- Identify feature priorities
- Understand competitive positioning
- Measure marketing effectiveness

## Customization Guide

### Modify Sentiment Categories
Change the overall sentiment options:
```csharp
"overallSentiment": "Very Positive, Positive, Neutral, Negative, Very Negative"
```

### Add Additional Fields
Extend the JSON structure:
```csharp
var sentimentAnalysisPrompt = @"Analyze the sentiment...
{{
  ""overallSentiment"": ""..."",
  ""positiveAspects"": [...],
  ""negativeAspects"": [...],
  ""emotionalTone"": ""..."",
  ""recommendationScore"": ""1-10"",
  ""keyTopics"": [""list of main topics discussed""]
}}";
```

### Focus on Specific Aspects
Tailor analysis to specific features:
```csharp
var sentimentAnalysisPrompt = @"Analyze product review focusing on:
- Product Quality
- Customer Service
- Value for Money
- Delivery Experience
...";
```

### Add Your Own Reviews
Replace or extend the `productReviews` array:
```csharp
var productReviews = new[]
{
    "Your custom product review here...",
    // ... more reviews
};
```

### Adjust Analysis Depth
For more detailed analysis:
```csharp
new ChatOptions
{
    Temperature = 0.1f,
    MaxOutputTokens = 300, // Increased for more detailed aspects
}
```

## JSON Response Parsing

The output is valid JSON that can be parsed programmatically:

```csharp
using System.Text.Json;

// Parse the JSON response
var sentimentData = JsonSerializer.Deserialize<SentimentResponse>(sentimentResponse.Text);

// Access individual fields
Console.WriteLine($"Sentiment: {sentimentData.OverallSentiment}");
Console.WriteLine($"Positive Count: {sentimentData.PositiveAspects.Length}");
```

Define a model class:
```csharp
public class SentimentResponse
{
    public string OverallSentiment { get; set; }
    public string[] PositiveAspects { get; set; }
    public string[] NegativeAspects { get; set; }
    public string EmotionalTone { get; set; }
}
```

## Token Usage and Cost Management

Each analysis shows token usage:
```
Tokens used: in=156, out=78
```

**Cost Management Tips:**
- Use `gpt-4o-mini` for standard analysis (cost-effective)
- Limit `MaxOutputTokens` to control response length
- Batch multiple reviews in one session
- Cache results for duplicate reviews

## Troubleshooting

### Error: "GitHub token not found in user secrets"
**Solution:** Run the user secrets configuration command:
```bash
dotnet user-secrets set "GitHubModels:Token" "your-token-here"
```

### Error: "HTTP 401 Unauthorized"
**Solution:** Your GitHub token may be invalid or expired. Generate a new token.

### Invalid JSON Response
**Solution:** 
1. Ensure prompt explicitly requests "Return only valid JSON"
2. Increase `MaxOutputTokens` if response is truncated
3. Use lower temperature (0.0-0.1) for more consistent formatting

### Missing Aspects in Analysis
**Solution:**
1. Use more capable model like `gpt-4o`
2. Increase `MaxOutputTokens` to 250-300
3. Modify prompt to request more detailed breakdown

### Inconsistent Sentiment Classification
**Solution:** Lower temperature to 0.0 for more deterministic results:
```csharp
Temperature = 0.0f
```

## Best Practices

1. **Low Temperature**: Use 0.0-0.2 for consistent, accurate sentiment analysis
2. **Structured Output**: Always request JSON for easy parsing
3. **Clear Schema**: Define exact JSON structure in prompt
4. **Validate JSON**: Parse and validate JSON responses programmatically
5. **Token Monitoring**: Track usage to manage costs
6. **Batch Processing**: Process multiple reviews efficiently
7. **Error Handling**: Handle malformed JSON responses gracefully

## Advanced Features

### Sentiment Aggregation
Aggregate multiple reviews:
```csharp
var allSentiments = new List<string>();
var allPositives = new List<string>();
var allNegatives = new List<string>();

// Collect from multiple reviews
// Calculate statistics
var positivePercent = allSentiments.Count(s => s == "Positive") * 100.0 / allSentiments.Count;
```

### Trend Analysis
Track sentiment over time:
```csharp
var sentimentByDate = reviews
    .GroupBy(r => r.Date)
    .Select(g => new { Date = g.Key, AvgSentiment = CalculateAvg(g) });
```

### Aspect-Based Sentiment
Analyze specific product aspects separately:
- Screen Quality → Positive
- Battery Life → Positive
- Customer Service → Negative

## Performance Tips

- Use `gpt-4o-mini` for standard sentiment analysis (fast, cost-effective)
- Use `gpt-4o` for nuanced emotional analysis
- Keep reviews under 1000 tokens for best results
- Process multiple reviews in batches
- Cache common analyses

## Related Projects

- **TextCompletion** - Basic chat completion with streaming support
- **Classification** - Text classification into categories
- **Summarization** - Text summarization with bullet points

## License
MIT License

## Support
For issues or questions, please refer to the main project documentation or GitHub Models documentation.
