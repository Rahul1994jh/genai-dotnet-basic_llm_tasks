# Basic LLM Tasks with .NET

## Overview
This solution contains a collection of .NET console applications demonstrating fundamental large language model (LLM) tasks using GitHub Models API and Microsoft.Extensions.AI. Each project showcases a different core NLP capability, providing practical examples for building AI-powered applications.

## 🎯 Projects

### 1. **[SentimentAnalysis](./SentimentAnalysis/README.md)**
Analyzes product reviews and customer feedback to extract:
- Overall sentiment (Positive, Negative, Neutral, Mixed)
- Positive and negative aspects
- Emotional tone analysis
- Structured JSON output

**Use Cases**: Customer feedback analysis, brand reputation management, market research

### 2. **[Summarization](./Summarization/README.md)**
Condenses long-form text into concise bullet-point summaries:
- Extracts main points from articles and documents
- Generates 2-5 key highlights
- Preserves context and accuracy
- Token usage tracking

**Use Cases**: Document processing, content curation, information digestion

### 3. **[Classification](./Classification/README.md)**
Automatically categorizes user feedback into predefined categories:
- Complaint
- Suggestion
- Praise
- Other

**Use Cases**: Support ticket routing, customer feedback analysis, content moderation

### 4. **[TextCompletion](./TextCompletion/README.md)**
Demonstrates fundamental chat completion capabilities:
- Non-streaming responses (complete response)
- Streaming responses (real-time token-by-token)
- ChatOptions configuration
- Token usage monitoring

**Use Cases**: Chatbots, virtual assistants, question answering, content generation

### 5. **[TextExtraction](./TextExtraction/README.md)**
Extracts structured information from unstructured car listing text:
- Strongly-typed data extraction using `GetResponseAsync<T>`
- Parse make, model, year, mileage, pricing details
- Handle Sale, Lease, and Rent availability types
- Batch processing of multiple listings
- Graceful handling of missing data with nullable types

**Use Cases**: Automotive marketplace data processing, price comparison systems, inventory management, classified ads parsing

## 🏗️ Common Architecture

All projects share a consistent architecture:

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
│          Task-Specific Logic            │
│  (Sentiment/Summarization/etc.)         │
└─────────────────────────────────────────┘
```

## 🚀 Getting Started

### Prerequisites
- .NET 10 SDK
- GitHub personal access token with GitHub Models access
- Visual Studio 2022 or later (or any .NET-compatible IDE)

### Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Rahul1994jh/genai-dotnet-basic_llm_tasks.git
   cd basic_llm_tasks
   ```

2. **Configure User Secrets**
   
   Each project requires a GitHub personal access token. Set it up for each project:
   
   ```bash
   cd SentimentAnalysis
   dotnet user-secrets init
   dotnet user-secrets set "GitHubToken" "your-github-token-here"
   ```
   
   Repeat for each project (Summarization, Classification, TextCompletion, TextExtraction).

3. **Configure Model Settings** (Optional)
   
   Each project has an `appsettings.json` file where you can customize:
   - AI model (default: gpt-4o-mini)
   - GitHub Models endpoint
   - Project-specific settings

4. **Run a Project**
   ```bash
   cd SentimentAnalysis
   dotnet run
   ```

### Getting a GitHub Token

1. Go to [GitHub Settings > Developer settings > Personal access tokens](https://github.com/settings/tokens)
2. Generate a new token (classic)
3. Enable access to GitHub Models (if available in beta)
4. Copy the token and use it in user secrets

## 🛠️ Technologies Used

- **.NET 10**: Latest .NET framework
- **Microsoft.Extensions.AI**: Unified AI abstraction layer
- **Azure.AI.Inference**: GitHub Models API client
- **Microsoft.Extensions.Configuration**: Configuration management
- **User Secrets**: Secure credential storage

## 📦 NuGet Packages

Each project uses the following packages:
```xml
<PackageReference Include="Azure.AI.Inference" Version="1.0.0-beta.2" />
<PackageReference Include="Microsoft.Extensions.AI" Version="9.0.1-preview.1.24570.5" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="9.0.0" />
```

## 🔐 Security Best Practices

- ✅ Use **User Secrets** for local development (never commit tokens)
- ✅ Use **Azure Key Vault** or environment variables for production
- ✅ Rotate API keys regularly
- ✅ Follow the principle of least privilege for token permissions

## 📝 Project Structure

```
basic_llm_tasks/
├── SentimentAnalysis/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── SentimentAnalysis.csproj
│   └── README.md
├── Summarization/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Summarization.csproj
│   └── README.md
├── Classification/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Classification.csproj
│   └── README.md
├── TextCompletion/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── TextCompletion.csproj
│   └── README.md
├── TextExtraction/
│   ├── Program.cs
│   ├── CarDetails.cs
│   ├── appsettings.json
│   ├── TextExtraction.csproj
│   └── README.md
└── README.md (this file)
```

## 🎓 Learning Path

We recommend exploring the projects in this order:

1. **TextCompletion** - Start here to understand basic LLM interaction
2. **Classification** - Learn about structured output and categorization
3. **Summarization** - Explore content transformation
4. **SentimentAnalysis** - Master complex structured analysis with JSON output
5. **TextExtraction** - Advanced strongly-typed extraction with custom models

## 🔗 Additional Resources

- [GitHub Models Documentation](https://docs.github.com/en/github-models)
- [Microsoft.Extensions.AI Documentation](https://devblogs.microsoft.com/dotnet/announcing-microsoft-extensions-ai-preview/)
- [Azure AI Inference SDK](https://learn.microsoft.com/en-us/azure/ai-services/)
- [.NET AI Samples](https://github.com/dotnet/ai-samples)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is provided as-is for educational purposes.

## 👤 Author

Created by [Rahul1994jh](https://github.com/Rahul1994jh)

---

**Happy Coding!** 🚀 If you find these examples helpful, please consider giving the repository a ⭐
