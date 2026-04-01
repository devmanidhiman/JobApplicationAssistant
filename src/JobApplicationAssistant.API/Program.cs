// src/JobApplicationAssistant.API/Program.cs
using Anthropic;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Infrastructure.Claude;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Anthropic client — reads ANTHROPIC_API_KEY from environment automatically
builder.Services.AddSingleton<AnthropicClient>();

// Register our abstraction
builder.Services.AddScoped<IClaudeService, ClaudeService>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/app-.txt",
        rollingInterval: RollingInterval.Day,    // new file each day
        retainedFileCountLimit: 7                // keep last 7 days
    )
    .CreateLogger();

var app = builder.Build();
builder.Host.UseSerilog();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();