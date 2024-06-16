var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", (IConfiguration configuration) =>
    Results.Ok(configuration.GetValue<string>("Providers:OpenAI:ApiKey")));

app.Run();
