using JobApplicationAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationAssistant.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<JobApplicationEntity> JobApplications => Set<JobApplicationEntity>();
    public DbSet<PipelineResultEntity> PipelineResults => Set<PipelineResultEntity>();
    public DbSet<PipelineErrorEntity> PipelineErrors => Set<PipelineErrorEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
