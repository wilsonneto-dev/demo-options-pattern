var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


public class OpenAiGateway
{
    public Task<string> ExecutePrompt(string prompt)
    {
        return Task.FromResult("not implemented");
    }
}