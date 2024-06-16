using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IValidateOptions<OpenAiSettings>, OpenAiSettingsValidate>();

builder.Services.AddOptionsWithValidateOnStart<OpenAiSettings>()
    .Bind(builder.Configuration.GetSection("Providers:OpenAI"));

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
    private readonly string _baseAddress = openAiSettings.Value.BaseAddress;
    private readonly string _apiKey = openAiSettings.Value.ApiKey;
    
    public Task<string> ExecutePrompt(string prompt) => 
        Task.FromResult($"BaseAddress: {_baseAddress} / Api Key: {_apiKey}");
}

public class OpenAiSettingsValidate : IValidateOptions<OpenAiSettings>
{
    public ValidateOptionsResult Validate(string? name, OpenAiSettings options)
    {
        var errors = new List<string>(); 
        if(string.IsNullOrWhiteSpace(options.BaseAddress))
            errors.Add("BaseAddress not found");
        if(string.IsNullOrWhiteSpace(options.ApiKey))
            errors.Add("ApiKey not found");

        return errors.Count > 0 ? 
            ValidateOptionsResult.Fail(string.Join(';', errors)) 
            : ValidateOptionsResult.Success;
    }
}

public class OpenAiSettings
{
    public string BaseAddress { get; init; } = "";
    
    public string ApiKey { get; init; } = "";
}