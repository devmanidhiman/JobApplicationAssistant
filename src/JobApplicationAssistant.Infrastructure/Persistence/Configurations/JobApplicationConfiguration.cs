using JobApplicationAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationAssistant.Infrastructure.Persistence.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplicationEntity>
{
    public void Configure(EntityTypeBuilder<JobApplicationEntity> builder)
    {
        builder.ToTable("job_applications");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        builder.Property(e => e.JobDescription).HasColumnName("job_description").IsRequired();
        builder.Property(e => e.ResumeText).HasColumnName("resume_text").IsRequired();
        builder.Property(e => e.Status).HasColumnName("status").IsRequired().HasDefaultValue("completed");
    }
}