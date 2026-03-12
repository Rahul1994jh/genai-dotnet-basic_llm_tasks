using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2");


#region Sentiment Analysis

Console.WriteLine("=== SENTIMENT ANALYSIS ===");
Console.WriteLine();

var sentimentAnalysisPrompt =
@"Analyze the sentiment of the following product review.
Provide the analysis as a JSON object with the following structure:
{{
  ""overallSentiment"": ""Positive, Negative, Neutral, or Mixed"",
  ""positiveAspects"": [""list of positive aspects""],
  ""negativeAspects"": [""list of negative aspects""],
  ""emotionalTone"": ""description of emotional tone""
}}
Review: '{0}'
Return only valid JSON, no additional text.";

// Create example product reviews to analyze
var productReviews = new[]
{
    "This laptop is absolutely amazing! The battery life lasts all day, the screen is crystal clear, and it's incredibly fast. Best purchase I've made this year. Highly recommend to anyone looking for a reliable machine.",

    "I'm very disappointed with this coffee maker. It broke after just two weeks of use, and the coffee tastes burnt. Customer service was unhelpful and refused to replace it. Complete waste of money.",

    "The headphones have great sound quality and are comfortable to wear for long periods. However, the Bluetooth connection drops occasionally, which is annoying. For the price, they're decent but not perfect.",

    "Received my order today and I'm blown away! The quality exceeds my expectations, shipping was fast, and it came beautifully packaged. Will definitely be ordering from this company again!",

    "It's okay, I guess. Does what it's supposed to do but nothing special. The design is pretty basic and feels a bit cheap. Probably wouldn't buy it again, but it works for now."
};

// Process each product review
foreach (var review in productReviews)
{
    Console.WriteLine($"Review: \"{review}\"");
    Console.WriteLine();

    var formattedPrompt = string.Format(sentimentAnalysisPrompt, review);
    var sentimentResponse = await client.GetResponseAsync(formattedPrompt, new ChatOptions
    {
        Temperature = 0.1f,
        MaxOutputTokens = 200,
    });

    Console.WriteLine($"Sentiment Analysis:");
    Console.WriteLine(sentimentResponse.Text);
    Console.WriteLine($"Tokens used: in={sentimentResponse.Usage?.InputTokenCount ?? 0}, out={sentimentResponse.Usage?.OutputTokenCount ?? 0}");
    Console.WriteLine(new string('-', 120));
    Console.WriteLine();
}

#endregion