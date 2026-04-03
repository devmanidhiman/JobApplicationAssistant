// src/JobApplicationAssistant.API/Program.cs
using Anthropic;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Infrastructure.Claude;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,    // new file each day
        retainedFileCountLimit: 7                // keep last 7 days
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Anthropic client — reads ANTHROPIC_API_KEY from environment automatically
builder.Services.AddSingleton<AnthropicClient>(_ =>
{
    var apiKey = builder.Configuration["ANTHROPIC_API_KEY"]
        ?? throw new InvalidOperationException("ANTHROPIC_API_KEY environment variable is not set.");

    return new AnthropicClient(new Anthropic.Core.ClientOptions { ApiKey = apiKey });
});

// Register our abstraction
builder.Services.AddScoped<IClaudeService, ClaudeService>();

builder.Host.UseSerilog();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();