using JobApplicationAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobApplicationAssistant.Infrastructure.Persistence.Configurations;

public class PipelineResultConfiguration : IEntityTypeConfiguration<PipelineResultEntity>
{
    public void Configure(EntityTypeBuilder<PipelineResultEntity> builder)
    {
        builder.ToTable("pipeline_results");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.JobApplicationId).HasColumnName("job_application_id");
        builder.Property(e => e.SkillExtraction).HasColumnName("skill_extraction").HasColumnType("jsonb");
        builder.Property(e => e.ResumeMatch).HasColumnName("resume_match").HasColumnType("jsonb");
        builder.Property(e => e.ResumeRewrite).HasColumnName("resume_rewrite").HasColumnType("jsonb");
        builder.Property(e => e.CoverLetter).HasColumnName("cover_letter").HasColumnType("jsonb");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        builder.HasOne(e => e.JobApplication)
            .WithMany()
            .HasForeignKey(e => e.JobApplicationId);
    }
}