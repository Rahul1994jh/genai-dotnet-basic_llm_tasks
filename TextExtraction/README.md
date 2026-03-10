# Text Extraction with GitHub Models

## Overview
This project demonstrates text extraction using GitHub Models API and Microsoft.Extensions.AI. It automatically extracts structured information from unstructured car listing text, parsing details like make, model, year, mileage, price, availability type, features, and more.

## Basic Concept
Text extraction (also known as information extraction or entity extraction) is an NLP task that identifies and extracts specific pieces of information from unstructured text. This implementation uses AI models to:
- Parse free-form car listing descriptions
- Extract specific data points (make, model, year, mileage, pricing, features, etc.)
- Return structured `CarDetails` objects for easy processing
- Handle missing or incomplete information gracefully with nullable types

This is valuable for:
- Automotive marketplace data processing
- Car listing aggregation and normalization
- Price comparison and analytics
- Inventory management systems
- Classified ads parsing
- Multi-source listing consolidation

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
│      Text Extraction Logic              │
│  - Define extraction prompt with schema │
│  - Process multiple car listings        │
│  - Extract using GetResponseAsync<T>    │
│  - Display structured CarDetails        │
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
   - Azure endpoint for GitHub Models API

4. **Client Initialization**
   - Creates OpenAI client for GitHub Models
   - Wraps as `IChatClient` for unified interface

5. **Text Extraction Logic**
   - Defines JSON schema for car listing extraction
   - Processes multiple car listings in a loop
   - Uses `GetResponseAsync<CarDetails>` for strongly-typed extraction
   - Validates and displays extracted data

## CarDetails Model

The extracted data is mapped to a strongly-typed `CarDetails` class:

```csharp
public class CarDetails
{
    public string Make { get; set; }                    // Car manufacturer/brand
    public string Model { get; set; }                   // Car model name
    public int? Year { get; set; }                      // Manufacturing year
    public double? Mileage { get; set; }                // Kilometers driven
    public double? Price { get; set; }                  // Price in lakhs
    public AvailabilityType? AvailabilityType { get; set; }  // Sale, Lease, or Rent
    public double? PricePerMonth { get; set; }          // Monthly lease price
    public double? PricePerDay { get; set; }            // Daily rent price
    public string[]? Features { get; set; }             // Notable features
    public string? Location { get; set; }               // Location if mentioned
    public string ShortSummary { get; set; }            // Brief summary (10-15 words)
    public int? OwnerCount { get; set; }                // Number of previous owners
}

## Sample Input

```
Check out this stylish Honda City 2018 model for sale, clocked only 30,000 km!
Single owner, showroom condition, insurance valid. Yours for just ₹6.5 lakh.
```

## Sample Output

```json
{
  "Make": "Honda",
  "Model": "City",
  "Year": 2018,
  "Mileage": 30000,
  "Price": 6.5,
  "AvailabilityType": "Sale",
  "PricePerMonth": null,
  "PricePerDay": null,
  "Features": [
    "Single owner",
    "Showroom condition",
    "Insurance valid"
  ],
  "Location": null,
  "ShortSummary": "Honda City 2018, single owner, showroom condition for sale",
  "OwnerCount": 1
}
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
Store your GitHub token securely using user secrets:
```bash
dotnet user-secrets init
dotnet user-secrets set "GitHubModels:Token" "your-github-token-here"
```

## Usage

1. **Set up user secrets** with your GitHub token (shared with TextCompletion project)
2. **Run the application**:
   ```bash
   dotnet run
   ```
3. **View extracted information** in structured JSON format

## Customization

### Extract Different Car Details
Modify the extraction prompt in `Program.cs` to specify different fields or add custom requirements:
```csharp
var prompt = @"Extract the following details from the car listing:
{
  ""Make"": ""string"",
  ""Model"": ""string"",
  ""FuelType"": ""string - Petrol/Diesel/Electric/Hybrid"",
  ""Transmission"": ""string - Manual/Automatic"",
  ""Color"": ""string""
}";
```

### Process Multiple Listings
The application processes a list of car listings in a loop:
```csharp
var carListings = new List<string>
{
    "Your car listing 1...",
    "Your car listing 2...",
    // Add more listings
};
```

### Customize Output Format
Control JSON serialization with options:
```csharp
var serializerOptions = new System.Text.Json.JsonSerializerOptions 
{ 
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
```

### Use Different AI Models
Update `appsettings.json` to use different models:
- `gpt-4o` - Most capable, higher cost
- `gpt-4o-mini` - Balanced performance and cost (default)
- `o1-preview` - Advanced reasoning
- `o1-mini` - Faster reasoning model

## Key Features

- ✅ **Structured Extraction**: Converts unstructured car listings to `CarDetails` objects
- ✅ **Flexible Schema**: Define custom fields for different extraction needs
- ✅ **Strongly Typed**: Uses `GetResponseAsync<CarDetails>` for type-safe extraction
- ✅ **Missing Data Handling**: Nullable properties handle incomplete information
- ✅ **Batch Processing**: Process multiple listings in a single run
- ✅ **Shared Configuration**: Uses same secrets and config as other projects
- ✅ **Multi-format Support**: Handles Sale, Lease, and Rent listings

## Common Use Cases

### 1. Car Marketplace Data Processing
Extract and normalize listing data from various sources:
- Make, model, year identification
- Price extraction and conversion
- Mileage and condition parsing
- Feature list compilation

### 2. Price Comparison Systems
Build car comparison tools:
- Standardize pricing across platforms
- Compare lease vs. rent vs. sale options
- Track market prices by model/year

### 3. Inventory Management
Automate dealership inventory:
- Import listings from multiple sources
- Standardize data formats
- Track availability types
- Update pricing information

### 4. Classified Ads Parsing
Process automotive classified ads:
- Extract key details from free-form text
- Identify location and contact information
- Parse complex pricing structures
- Categorize by availability type

## Best Practices

1. **Specify Clear Schema**: Define exact JSON schema in your prompt for consistent extraction
2. **Use Strongly-Typed Models**: Define C# classes matching your extraction schema
3. **Handle Missing Data**: Use nullable types for optional fields
4. **Validate Output**: Check `TryGetResult()` before accessing extracted data
5. **Batch Process**: Process multiple items in loops for efficiency
6. **Test Variations**: Ensure your prompt handles different listing formats
7. **Enum Conversion**: Use `JsonStringEnumConverter` for string-to-enum mapping

## Example Listings Processed

The application processes 9 different car listings demonstrating:
- **Sale listings**: Honda City, Maruti Swift, Ford EcoSport, Maruti Wagon R
- **Lease listings**: Hyundai Creta, Hyundai Verna
- **Rent listings**: Toyota Innova Crysta, Mahindra Scorpio, BMW 3 Series

Each listing is parsed to extract all available information, with missing fields set to `null`.

## Error Handling

The application includes error handling for:
- Missing GitHub token
- Invalid configuration
- API connection failures
- Malformed extraction responses

## Dependencies

- **Microsoft.Extensions.AI**: Unified AI abstraction layer
- **Microsoft.Extensions.AI.OpenAI**: OpenAI integration
- **Azure.AI.Inference**: GitHub Models API client
- **Microsoft.Extensions.Configuration**: Configuration management

## Token Usage

Text extraction typically uses:
- **Input tokens**: 100-500 (depending on text length)
- **Output tokens**: 50-200 (for structured JSON)

Monitor token usage to optimize costs and performance.

## Troubleshooting

### Token Not Found
```
GitHub token not found in user secrets. Please configure 'GitHubModels:Token'.
```
**Solution**: Run `dotnet user-secrets set "GitHubModels:Token" "your-token"`

### Invalid JSON Output
If the model returns non-JSON text, adjust your prompt:
```csharp
string extractionPrompt = @$"Extract information and return ONLY a valid JSON object. 
Do not include any explanatory text before or after the JSON.";
```

### Poor Extraction Quality
- Increase model capability (use gpt-4o instead of gpt-4o-mini)
- Provide examples in your prompt (few-shot learning)
- Clean and format input text before extraction

## Related Projects

- **[TextCompletion](../TextCompletion/README.md)** - Basic chat completion
- **[Classification](../Classification/README.md)** - Text categorization
- **[SentimentAnalysis](../SentimentAnalysis/README.md)** - Sentiment extraction
- **[Summarization](../Summarization/README.md)** - Text summarization

## Additional Resources

- [GitHub Models Documentation](https://docs.github.com/en/github-models)
- [Microsoft.Extensions.AI](https://devblogs.microsoft.com/dotnet/announcing-microsoft-extensions-ai-preview/)
- [Information Extraction NLP](https://en.wikipedia.org/wiki/Information_extraction)

---

**Happy Extracting!** 🚀
