using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using System.ClientModel;
using TextExtraction;

#region Configuration Setup

// Build configuration and load from multiple sources:
// 1. appsettings.json - Application settings (model, endpoint)
// 2. User secrets - Secure storage for API token
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

#endregion

#region Authentication

// Retrieve GitHub Models API token from user secrets
// Throws an exception if the token is not configured
ApiKeyCredential credential = new(configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("GitHub token not found in user secrets. Please configure 'GitHubModels:Token'."));

#endregion

#region Model Configuration

// Load model name from configuration (defaults to gpt-4o-mini if not specified)
// Available models: gpt-4o, gpt-4o-mini, o1-preview, o1-mini
string model = configuration["GitHubModels:Model"] ?? "gpt-4o-mini";

// Load endpoint from configuration
string endpoint = configuration["GitHubModels:Endpoint"] ?? "https://models.inference.ai.azure.com";

#endregion

#region Client Initialization

// Create OpenAI client configured for GitHub Models endpoint
// GitHub Models provides access to AI models through Azure's inference API
IChatClient chatClient = new OpenAIClient(credential, new OpenAIClientOptions
{
    Endpoint = new Uri(endpoint)
}).GetChatClient(model)
.AsIChatClient();

#endregion

#region Text Extraction

var prompt = @"Extract the following details from the car listing and return ONLY a valid JSON object matching this schema:
{
  ""Make"": ""string - car manufacturer/brand"",
  ""Model"": ""string - car model name"",
  ""Year"": number - manufacturing year,
  ""Mileage"": number - kilometers driven (numeric value only),
  ""Price"": number - price in lakhs (convert to numeric, e.g., 6.5 for ₹6.5 lakh),
  ""AvailabilityType"": ""string - one of: Sale, Lease, Rent"",
  ""PricePerMonth"": number - monthly lease price in rupees (null if not applicable),
  ""PricePerDay"": number - daily rent price in rupees (null if not applicable),
  ""Features"": ""array of short string - notable features or condition"",
  ""Location"": ""string - location if mentioned, otherwise null"",
  ""ShortSummary"": ""string - short summary in not more than 10-15 words to summarize this listing"",
  ""OwnerCount"": number - number of previous owners (null if not mentioned)
}
Extract all available information and use null for missing values. 
Return only the JSON object, no additional text.";

var carListings = new List<string>
{
    "Check out this stylish Honda City 2018 model for sale, clocked only 30,000 km!.Single owner, showroom condition, insurance valid. Yours for just ₹6.5 lakh.",
    "Drive home a Hyundai Creta SX 2020 — premium SUV with sunroof and leather seats. Available on easy monthly lease at ₹22,000. Perfect for professionals in Noida.",
    "Looking for a reliable daily drive? Grab this Maruti Swift VXi 2017, just 45,000 km driven.Compact, fuel-efficient, and priced at ₹3.25 lakh.",
    "Family trips made easy with Toyota Innova Crysta 2019.Spacious 7-seater, only 40,000 km, available for rent at ₹2,500/day.",
    "Own the road with Mahindra Scorpio S11 — rugged SUV, 2018 model.Available for self-drive rental at ₹2,200/day. Unlimited km, no hidden charges.",
    "Ford EcoSport Titanium 2016 — sporty compact SUV, 60,000 km driven.Well-maintained, priced at ₹4.75 lakh. Ready for a new owner in Noida.",
    "Turn heads with BMW 3 Series 2021 — luxury sedan in pristine condition.Rent it for ₹6,500/day. Ideal for weddings, corporate events, or weekend indulgence.",
    "Hyundai Verna 2018 — sleek sedan with automatic transmission.Lease option available at ₹20,000/month. Stylish, comfortable, and executive-ready.",
    "Maruti Wagon R 2015 — the perfect city car.Just 55,000 km, reliable and low-maintenance. Yours for ₹2.1 lakh.",
};

// Process each car listing and extract structured data
var extractedCarData = new List<CarDetails>();

Console.WriteLine("Processing car listings...\n");

var serializerOptions = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };

foreach (var listing in carListings)
{
    // Combine the extraction prompt with the specific car listing
    var userMessage = $"{prompt}\n\nCar Listing:\n{listing}";
    
    // Send the message to the AI model
    var response = await chatClient.GetResponseAsync<CarDetails>(userMessage);
    
    // Try to extract the structured CarDetails object from the response
    if (response.TryGetResult(out CarDetails? carDetails) && carDetails != null)
    {
        // Add to results list
        extractedCarData.Add(carDetails);
        
        // Display the results
        Console.WriteLine($"Listing: {listing[..Math.Min(100, listing.Length)]}...");
        Console.WriteLine($"Extracted: {System.Text.Json.JsonSerializer.Serialize(carDetails, serializerOptions)}");
        Console.WriteLine(new string('-', 200));
        Console.WriteLine();
    }
    else
    {
        // Handle extraction failure
        Console.WriteLine($"Failed to extract data from listing: {listing[..Math.Min(50, listing.Length)]}...");
        Console.WriteLine($"Error: Unable to parse response into CarDetails object");
        Console.WriteLine(new string('-', 80));
        Console.WriteLine();
    }
}

Console.WriteLine($"\nTotal listings processed: {extractedCarData.Count}");
#endregion
