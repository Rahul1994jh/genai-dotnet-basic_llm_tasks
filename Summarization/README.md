# Text Summarization with GitHub Models

## Overview
This project demonstrates text summarization using GitHub Models API and Microsoft.Extensions.AI. It automatically generates concise bullet-point summaries of long-form text, making complex information easier to digest.

## Basic Concept
Text summarization is an NLP task that condenses lengthy text into shorter versions while preserving key information and main ideas. This implementation uses AI models to:
- Extract main points from articles, reports, or documents
- Generate 2-5 bullet points highlighting key information
- Maintain context and accuracy of the original content
- Save reading time and improve information accessibility

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
│      Summarization Logic                │
│  - Format prompt with source text       │
│  - Request bullet-point summary         │
│  - Receive structured summary           │
│  - Display with token usage stats       │
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

5. **Summarization Engine**
   - Defines summarization prompt template
   - Processes multiple text examples
   - Returns structured bullet-point summaries
   - Tracks token usage for cost monitoring

## Setup Instructions

### Prerequisites
- .NET 10 SDK
- GitHub Personal Access Token with access to GitHub Models
- Visual Studio 2022 or VS Code

### Step 1: Install Dependencies
All required NuGet packages are already defined in `Summarization.csproj`:
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
cd Summarization
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
- `gpt-4o` - Most capable, better for complex texts
- `gpt-4o-mini` - Balanced performance and cost (recommended)
- `o1-preview` - Reasoning model for analytical summaries
- `o1-mini` - Smaller reasoning model

## Running the Example

### Option 1: Using dotnet CLI
```bash
cd Summarization
dotnet run
```

### Option 2: Using Visual Studio
1. Open the solution in Visual Studio
2. Set `Summarization` as the startup project
3. Press F5 or click Run

### Option 3: Run from solution root
```bash
dotnet run --project Summarization
```

## Expected Output

```
=== TEXT SUMMARIZATION ===

Original Text: The quarterly earnings report shows that our company exceeded expectations with a 15% increase in revenue compared to last year. This growth was primarily driven by strong sales in the Asia-Pacific region and the successful launch of our new product line. Operating expenses remained stable, and our profit margin improved from 12% to 14%. The board of directors has approved a dividend increase of 5% for shareholders.

Summary:
• Company revenue increased by 15% year-over-year, exceeding expectations
• Growth driven by strong Asia-Pacific sales and new product line launch
• Profit margin improved from 12% to 14% with stable operating expenses
• Board approved 5% dividend increase for shareholders
Tokens used: in=145, out=68
------------------------------------------------------------------------------------------------------------------------

[Additional examples...]
```

## How It Works

### 1. Summarization Prompt
The system uses a structured prompt that guides the AI model:
```
Summarize the following text as bullet points for easy understanding.
Provide 2-5 bullet points focusing on the main points and key information.
Text: '{source_text}'
```

### 2. Processing Flow
```
Source Text → Clean Whitespace → Format Prompt → Send to AI Model → Generate Summary → Display Results
```

### 3. Text Preprocessing
The system uses regex to normalize whitespace:
```csharp
System.Text.RegularExpressions.Regex.Replace(text.Trim(), @"\s+", " ")
```
This ensures clean, single-line display of source text.

### 4. ChatOptions Configuration
- **Temperature: 0.1** - Low temperature for consistent, factual summaries
- **MaxOutputTokens: 150** - Sufficient for 2-5 bullet points

## Example Use Cases

### 1. Business Reports
Summarize quarterly earnings, market analysis, or business intelligence reports into key takeaways.

### 2. Scientific Articles
Condense research papers or technical documents into digestible summaries.

### 3. News Articles
Extract main points from news stories for quick consumption.

### 4. Meeting Notes
Summarize lengthy meeting transcripts into action items and decisions.

### 5. Customer Feedback
Aggregate and summarize multiple customer reviews or feedback.

## Customization Guide

### Change Summary Format
Modify the prompt to request different formats:

```csharp
// For numbered summaries
var summaryPrompt = @"Summarize the following text as a numbered list.
Provide 3-5 key points.
Text: '{0}'";

// For paragraph summaries
var summaryPrompt = @"Summarize the following text in one concise paragraph.
Text: '{0}'";

// For executive summaries
var summaryPrompt = @"Create an executive summary with:
1. Main finding
2. Key implications
3. Recommended actions
Text: '{0}'";
```

### Adjust Summary Length
Control the number of bullet points:
```csharp
var summaryPrompt = @"Summarize the following text as exactly 3 bullet points.
Text: '{0}'";
```

Increase token limit for longer summaries:
```csharp
new ChatOptions
{
    Temperature = 0.1f,
    MaxOutputTokens = 300, // Increased for more detailed summaries
}
```

### Add Your Own Texts
Replace or add to the `textsToSummarize` array:
```csharp
var textsToSummarize = new[]
{
    @"Your custom long-form text here...",
    // ... more examples
};
```

### Change Output Style
Adjust temperature for different summary styles:
- **0.0-0.2**: Very factual, consistent (recommended)
- **0.3-0.5**: Slight variation in wording
- **0.6-1.0**: More creative rephrasing (may lose accuracy)

## Token Usage and Cost Management

Each summarization shows token usage:
```
Tokens used: in=145, out=68
```

**Input tokens** = Source text length  
**Output tokens** = Summary length

Monitor these to manage costs:
- Use `gpt-4o-mini` for cost-effective summaries
- Limit `MaxOutputTokens` to control costs
- Batch multiple summaries in one session

## Troubleshooting

### Error: "GitHub token not found in user secrets"
**Solution:** Run the user secrets configuration command:
```bash
dotnet user-secrets set "GitHubModels:Token" "your-token-here"
```

### Error: "HTTP 401 Unauthorized"
**Solution:** Your GitHub token may be invalid or expired. Generate a new token.

### Summaries are too short
**Solution:** Increase `MaxOutputTokens` or modify prompt to request more detail:
```csharp
MaxOutputTokens = 250
```

### Summaries are inconsistent
**Solution:** Lower temperature closer to 0.0:
```csharp
Temperature = 0.0f
```

### Summaries miss important points
**Solution:** 
1. Use a more capable model like `gpt-4o`
2. Modify prompt to emphasize specific aspects
3. Increase the number of bullet points requested

## Best Practices

1. **Low Temperature**: Use 0.0-0.2 for factual, consistent summaries
2. **Clear Instructions**: Specify exact number of bullet points desired
3. **Token Monitoring**: Track usage to manage costs
4. **Text Preprocessing**: Clean and normalize input text
5. **Appropriate Length**: Match MaxOutputTokens to summary complexity
6. **Batch Processing**: Process multiple texts in one session for efficiency

## Performance Tips

- Use `gpt-4o-mini` for standard summarization (fast, cost-effective)
- Use `gpt-4o` for complex technical or legal documents
- Keep source texts under 4000 tokens for best results
- Process multiple documents in batches

## Related Projects

- **TextCompletion** - Basic chat completion with streaming support
- **Classification** - Text classification into categories
- **SentimentAnalysis** - Product review sentiment analysis

## License
MIT License

## Support
For issues or questions, please refer to the main project documentation or GitHub Models documentation.
