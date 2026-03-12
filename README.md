# Basic LLM Tasks with .NET

## Overview
This solution contains a collection of .NET console applications demonstrating fundamental large language model (LLM) tasks using both GitHub Models API (cloud) and Ollama (local). Each project showcases different core NLP capabilities, providing practical examples for building AI-powered applications with flexibility in deployment options.

## 🎯 Projects

### GitHub Models (Cloud-Based)

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

### 6. **[ChatApplication](./ChatApplication/README.md)**
Full-featured interactive travel assistant chatbot:
- Multi-turn conversations with history
- System prompts with professional guardrails
- Real-time streaming responses
- Visual thinking animation
- Exit command support

**Use Cases**: Customer support bots, virtual assistants, domain-specific advisors, interactive Q&A systems

### Ollama (Local/Offline)

### 7. **[TextCompletion_Ollama](./TextCompletion_Ollama/README.md)**
Chat completion using Ollama running locally:
- Run Llama 3.2 on your own machine
- No API keys or internet required
- Streaming responses
- Complete privacy and zero cost

**Use Cases**: Offline AI, privacy-sensitive apps, development without API costs, edge computing

### 8. **[SentimentAnalysis_ollama](./SentimentAnalysis_ollama/README.md)**
Sentiment analysis using local Llama 3.2 model:
- Analyze reviews completely offline
- Extract sentiment without cloud dependency
- Batch process reviews locally
- Full data privacy

**Use Cases**: Privacy-sensitive feedback analysis, offline review processing, development and testing

## 🏗️ Common Architecture

### GitHub Models Projects
All cloud-based projects share this architecture:

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

### Ollama Projects
Local projects use simplified architecture:

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
│          Task-Specific Logic            │
│    (No config, keys, or secrets)        │
└─────────────────────────────────────────┘
```

## 🚀 Getting Started

### Prerequisites

**For GitHub Models Projects:**
- .NET 10 SDK
- GitHub personal access token with GitHub Models access
- Visual Studio 2022 or later (or any .NET-compatible IDE)

**For Ollama Projects:**
- .NET 10 SDK
- Ollama installed locally ([ollama.ai](https://ollama.ai))
- Llama 3.2 model pulled: `ollama pull llama3.2`

### Setup Instructions

#### GitHub Models Projects

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Rahul1994jh/genai-dotnet-basic_llm_tasks.git
   cd basic_llm_tasks
   ```

2. **Configure User Secrets**

   Each GitHub Models project requires a token. Set it up for each project:

   ```bash
   cd SentimentAnalysis
   dotnet user-secrets init
   dotnet user-secrets set "GitHubModels:Token" "your-github-token-here"
   ```

   Repeat for: Summarization, Classification, TextCompletion, TextExtraction, ChatApplication.

3. **Run a Project**
   ```bash
   dotnet run
   ```

#### Ollama Projects

1. **Install Ollama**
   - Download from [ollama.ai](https://ollama.ai)
   - Install and start the server

2. **Pull the Model**
   ```bash
   ollama pull llama3.2
   ```

3. **Run a Project**
   ```bash
   cd TextCompletion_Ollama
   dotnet run
   ```

No configuration files or API keys needed! 🎉

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
3. Enable access to GitHub Models
4. Copy the token and use it in user secrets

### Installing Ollama

1. Visit [ollama.ai](https://ollama.ai) and download for your OS
2. Install and run: `ollama serve`
3. Pull models: `ollama pull llama3.2`
4. Verify: `ollama list`

## 🛠️ Technologies Used

- **.NET 10**: Latest .NET framework
- **Microsoft.Extensions.AI**: Unified AI abstraction layer
- **Azure.AI.Inference**: GitHub Models API client
- **OllamaSharp**: Local Ollama integration
- **Microsoft.Extensions.Configuration**: Configuration management
- **User Secrets**: Secure credential storage (GitHub Models projects)

## 📦 NuGet Packages

### GitHub Models Projects
```xml
<PackageReference Include="Azure.AI.Inference" Version="1.0.0-beta.2" />
<PackageReference Include="Microsoft.Extensions.AI" Version="9.0.1-preview.1.24570.5" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="9.0.0" />
```

### Ollama Projects
```xml
<PackageReference Include="Microsoft.Extensions.AI" Version="9.0.1-preview.1.24570.5" />
<PackageReference Include="OllamaSharp" Version="3.1.0" />
```

## 🔐 Security Best Practices

**For GitHub Models Projects:**
- ✅ Use **User Secrets** for local development (never commit tokens)
- ✅ Use **Azure Key Vault** or environment variables for production
- ✅ Rotate API keys regularly
- ✅ Follow the principle of least privilege for token permissions

**For Ollama Projects:**
- ✅ Runs completely locally - no API keys or secrets needed
- ✅ Data never leaves your machine - full privacy
- ✅ No authentication layer required
- ✅ Ideal for privacy-sensitive applications

## 🤔 Choosing Between GitHub Models and Ollama

| Criteria | GitHub Models (Cloud) | Ollama (Local) |
|----------|----------------------|----------------|
| **Setup** | ✅ Quick (just API key) | ⚠️ Download models (~4GB+) |
| **Internet** | ❌ Required | ✅ Not required |
| **Cost** | ⚠️ Token-based | ✅ Free forever |
| **Privacy** | ⚠️ Data sent to cloud | ✅ 100% local |
| **Speed** | ✅ Fast cloud GPUs | ⚠️ Hardware dependent |
| **Model Quality** | ✅ GPT-4o, GPT-4o-mini | ⚠️ Llama 3.2 (good) |
| **Accuracy** | ✅ Excellent | ⚠️ Good |
| **Best For** | Production apps, best results | Development, privacy, offline |

**Use GitHub Models when:**
- You need the best accuracy
- Speed is critical
- You're building production applications
- You want access to latest models

**Use Ollama when:**
- Privacy is paramount
- You want zero costs
- You need offline capability
- You're learning/experimenting
- You have good hardware (GPU recommended)

## 📝 Project Structure

```
basic_llm_tasks/
├── GitHub Models (Cloud) Projects/
│   ├── SentimentAnalysis/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── SentimentAnalysis.csproj
│   │   └── README.md
│   ├── Summarization/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── Summarization.csproj
│   │   └── README.md
│   ├── Classification/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── Classification.csproj
│   │   └── README.md
│   ├── TextCompletion/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── TextCompletion.csproj
│   │   └── README.md
│   ├── TextExtraction/
│   │   ├── Program.cs
│   │   ├── CarDetails.cs
│   │   ├── appsettings.json
│   │   ├── TextExtraction.csproj
│   │   └── README.md
│   └── ChatApplication/
│       ├── Program.cs
│       ├── appsettings.json
│       ├── ChatApplication.csproj
│       └── README.md
├── Ollama (Local) Projects/
│   ├── TextCompletion_Ollama/
│   │   ├── Program.cs
│   │   ├── TextCompletion_Ollama.csproj
│   │   └── README.md
│   └── SentimentAnalysis_ollama/
│       ├── Program.cs
│       ├── SentimentAnalysis_Ollama.csproj
│       └── README.md
└── README.md (this file)
```

## 🎓 Learning Path

We recommend exploring the projects in this order:

### Beginner Path (Cloud)
1. **TextCompletion** - Start here to understand basic LLM interaction
2. **Classification** - Learn about structured output and categorization
3. **Summarization** - Explore content transformation
4. **SentimentAnalysis** - Master complex structured analysis with JSON output
5. **TextExtraction** - Advanced strongly-typed extraction with custom models
6. **ChatApplication** - Build full conversational applications with history

### Local/Privacy-First Path
1. **TextCompletion_Ollama** - Get started with local AI (no API keys!)
2. **SentimentAnalysis_ollama** - Offline sentiment analysis
3. Then explore GitHub Models projects for comparison

### Comparison Path
Try both versions of the same task:
- **TextCompletion** vs **TextCompletion_Ollama**
- **SentimentAnalysis** vs **SentimentAnalysis_ollama**

Understand trade-offs between cloud (accuracy, speed) vs local (privacy, cost).

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
