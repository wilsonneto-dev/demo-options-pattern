using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("Providers:OpenAI"));

builder.Services.AddTransient<OpenAiGateway>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/", async (EndpointInput input, OpenAiGateway openAiGateway) =>
{
    var response = await openAiGateway.ExecutePrompt(input.Prompt);
    return Results.Ok(new EndpointOutput(response));
});

app.Run();


public record EndpointInput(string Prompt);
public record EndpointOutput(string Response);


internal class OpenAiGateway(IOptions<OpenAiSettings> openAiSettings)
{
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress
        ?? throw new ArgumentException("Base Address not found...");

    private readonly string _apiKey = openAiSettings.Value.ApiKey
        ?? throw new ArgumentException("Api key not found...");
    
    public Task<string> ExecutePrompt(string prompt) => 
        Task.FromResult($"BaseAddress: {_baseAddress} / Api Key: {_apiKey}");
}

public class OpenAiSettings
{
    public string? BaseAddress { get; set; }
    public string? ApiKey { get; set; }
}