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
        builder.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        builder.Property(e => e.SkillExtraction).HasColumnType("jsonb");
        builder.Property(e => e.ResumeMatch).HasColumnType("jsonb");
        builder.Property(e => e.ResumeRewrite).HasColumnType("jsonb");
        builder.Property(e => e.CoverLetter).HasColumnType("jsonb");
        builder.HasOne(e => e.JobApplication)
               .WithMany()
               .HasForeignKey(e => e.JobApplicationId);
    }
}