using Microsoft.Extensions.AI;
using OllamaSharp;

IChatClient client = new OllamaApiClient("http://localhost:11434", "llama3.2");

#region Chat

Console.WriteLine("=== CHAT COMPLETION ===");
Console.WriteLine();

#region User Prompt
string prompt = "What is the meaning of life?";
Console.WriteLine($"user >>> {prompt}");
Console.WriteLine();
#endregion

#region Non Streaming Response

//// Send a chat completion request and get the response
//var response = await client.GetResponseAsync(prompt, new ChatOptions
//{
//    Temperature = 0.1f, // Controls randomness of the output (0.0 - 1.0)   
//    MaxOutputTokens = 300, // Limits the length of the response
//});

//// Display the model's response
//Console.WriteLine($"assistant >>> {response}");
//Console.WriteLine($"Tokens used in={response.Usage?.InputTokenCount ?? 0}, out={response.Usage?.OutputTokenCount ?? 0}");
//Console.WriteLine();

#endregion

#region Streaming Response

Console.WriteLine("--- Streaming Response ---");
Console.WriteLine();

// Send a chat completion request and get the streaming response
var responseStream = client.GetStreamingResponseAsync(prompt, new ChatOptions
{
    Temperature = 0.5f,
    MaxOutputTokens = 300,
});

// Display the model's response
Console.Write($"assistant >>> ");
await foreach (var message in responseStream)
{
    Console.Write(message.Text);
}
Console.WriteLine();

#endregion

#endregion