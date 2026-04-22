using JobApplicationAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationAssistant.Infrastructure.Persistence.Configurations;

public class PipelineErrorConfiguration : IEntityTypeConfiguration<PipelineErrorEntity>
{
    public void Configure(EntityTypeBuilder<PipelineErrorEntity> builder)
    {
        builder.ToTable("pipeline_errors");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.JobApplicationId).HasColumnName("job_application_id");
        builder.Property(e => e.FailedStep).HasColumnName("failed_step").IsRequired();
        builder.Property(e => e.ErrorMessage).HasColumnName("error_message").IsRequired();
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        builder.HasOne(e => e.JobApplication)
               .WithMany()
               .HasForeignKey(e => e.JobApplicationId);
    }
}