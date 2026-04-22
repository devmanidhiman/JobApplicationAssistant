// src/JobApplicationAssistant.API/Program.cs
using Anthropic;
using JobApplicationAssistant.Core;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Infrastructure.Claude;
using JobApplicationAssistant.Infrastructure.Persistence;
using JobApplicationAssistant.Infrastructure.Pipeline;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddSingleton<AnthropicClient>(_ =>
{
    var apiKey = builder.Configuration["ANTHROPIC_API_KEY"]
        ?? throw new InvalidOperationException("ANTHROPIC_API_KEY environment variable is not set.");

    return new AnthropicClient(new Anthropic.Core.ClientOptions { ApiKey = apiKey });
});

// Register our abstraction
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IClaudeService, ClaudeService>();
builder.Services.AddScoped<ISkillExtractionService, SkillExtractionService>();
builder.Services.AddScoped<IResumeMatchService, ResumeMatchService>();
builder.Services.AddScoped<IResumeRewriteService, ResumeRewriteService>();
builder.Services.AddScoped<ICoverLetterService, CoverLetterService>();
builder.Services.AddScoped<IPipelineOrchestrator, PipelineOrchestrator>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();

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