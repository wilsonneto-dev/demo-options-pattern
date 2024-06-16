## Options Pattern demo

This repo is an example of the Options Pattern way of handling .Net app configurations, a more modular and with better separation of concerns way.

```csharp
builder.Services.AddSingleton<IValidateOptions<OpenAiSettings>, OpenAiSettingsValidate>();

builder.Services.AddOptionsWithValidateOnStart<OpenAiSettings>()
    .Bind(builder.Configuration.GetSection("Providers:OpenAI"));

// ...

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
```

This demo uses:
- .Net 9

<br /><br />
---

| [<img src="https://github.com/wilsonneto-dev.png" width="75px;"/>][1] |
| :-: |
|[Wilson Neto][1]|


[1]: https://github.com/wilsonneto-dev
