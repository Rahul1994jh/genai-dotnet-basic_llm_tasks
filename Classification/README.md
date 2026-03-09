# Text Classification with GitHub Models

## Overview
This project demonstrates text classification using GitHub Models API and Microsoft.Extensions.AI. It automatically categorizes user feedback into predefined categories: Complaint, Suggestion, Praise, or Other.

## Basic Concept
Text classification is a natural language processing task where text is automatically assigned to one or more predefined categories. This implementation uses AI models to understand the context and sentiment of user feedback, making it valuable for:
- Customer feedback analysis
- Support ticket routing
- Content moderation
- User sentiment categorization

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
│      Classification Logic               │
│  - Format prompt with user text         │
│  - Send to AI model                     │
│  - Receive category classification      │
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

5. **Classification Engine**
   - Defines classification prompt template
   - Processes multiple user feedback examples
   - Returns single-word category classification

## Setup Instructions

### Prerequisites
- .NET 10 SDK
- GitHub Personal Access Token with access to GitHub Models
- Visual Studio 2022 or VS Code

### Step 1: Install Dependencies
All required NuGet packages are already defined in `Classification.csproj`:
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
cd Classification
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
- `gpt-4o` - Most capable, slower, higher cost
- `gpt-4o-mini` - Balanced performance and cost (recommended)
- `o1-preview` - Reasoning model preview
- `o1-mini` - Smaller reasoning model

## Running the Example

### Option 1: Using dotnet CLI
```bash
cd Classification
dotnet run
```

### Option 2: Using Visual Studio
1. Open the solution in Visual Studio
2. Set `Classification` as the startup project
3. Press F5 or click Run

### Option 3: Run from solution root
```bash
dotnet run --project Classification
```

## Expected Output

```
=== TEXT CLASSIFICATION ===

Text: "The app keeps crashing every time I try to upload a photo. This is really frustrating!"
Classification: Complaint

Text: "It would be great if you could add a dark mode option to the settings."
Classification: Suggestion

Text: "Your customer service team was incredibly helpful and resolved my issue within minutes. Thank you!"
Classification: Praise

Text: "When does the store close on Sundays?"
Classification: Other

Text: "The delivery was two weeks late and the packaging was damaged. Completely unacceptable."
Classification: Complaint
```

## How It Works

### 1. Classification Prompt
The system uses a carefully crafted prompt that instructs the AI model:
```
Classify the text into one category: 
Complaint, Suggestion, Praise, or Other.
Respond with only the category name.
Text: '{user_feedback}'
```

### 2. Processing Flow
```
User Feedback → Format Prompt → Send to AI Model → Parse Category → Display Result
```

### 3. ChatOptions Configuration
- **Temperature: 0.1** - Low temperature for consistent, deterministic classification
- **MaxOutputTokens: 50** - Limited tokens since we only expect a single word response

## Customization Guide

### Add New Categories
Modify the classification prompt in `Program.cs`:
```csharp
var classificationPrompt = @"Classify the text into one category: 
Complaint, Suggestion, Praise, Question, Bug Report, Feature Request, or Other.
Respond with only the category name.
Text: '{0}'";
```

### Add Your Own Examples
Add new feedback texts to the `userFeedbacks` array:
```csharp
var userFeedbacks = new[]
{
    "Your custom feedback text here",
    // ... more examples
};
```

### Change Model Temperature
Adjust the `Temperature` parameter for different classification behaviors:
- **0.0-0.2**: Very consistent, deterministic (recommended for classification)
- **0.3-0.5**: Slightly more varied responses
- **0.6-1.0**: More creative, less predictable (not recommended for classification)

## Troubleshooting

### Error: "GitHub token not found in user secrets"
**Solution:** Run the user secrets configuration command:
```bash
dotnet user-secrets set "GitHubModels:Token" "your-token-here"
```

### Error: "HTTP 401 Unauthorized"
**Solution:** Your GitHub token may be invalid or expired. Generate a new token.

### Error: "HTTP 400 Bad Request: unknown_model"
**Solution:** Check that the model name in `appsettings.json` is valid.

### Inconsistent Classifications
**Solution:** Lower the temperature value closer to 0.0 for more consistent results.

## Best Practices

1. **Use Low Temperature**: Classification tasks benefit from low temperature (0.0-0.2) for consistency
2. **Clear Categories**: Define clear, non-overlapping categories
3. **Provide Context**: Include enough context in your prompt for accurate classification
4. **Limit Output Tokens**: Use minimal `MaxOutputTokens` for single-word responses
5. **Test Edge Cases**: Test with ambiguous examples to understand model behavior

## Related Projects

- **TextCompletion** - Basic chat completion with streaming support
- **Summarization** - Text summarization with bullet points
- **SentimentAnalysis** - Product review sentiment analysis

## License
MIT License

## Support
For issues or questions, please refer to the main project documentation or GitHub Models documentation.
