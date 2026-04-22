using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnsToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pipeline_results_job_applications_JobApplicationId",
                table: "pipeline_results");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "pipeline_results",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SkillExtraction",
                table: "pipeline_results",
                newName: "skill_extraction");

            migrationBuilder.RenameColumn(
                name: "ResumeRewrite",
                table: "pipeline_results",
                newName: "resume_rewrite");

            migrationBuilder.RenameColumn(
                name: "ResumeMatch",
                table: "pipeline_results",
                newName: "resume_match");

            migrationBuilder.RenameColumn(
                name: "JobApplicationId",
                table: "pipeline_results",
                newName: "job_application_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "pipeline_results",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CoverLetter",
                table: "pipeline_results",
                newName: "cover_letter");

            migrationBuilder.RenameIndex(
                name: "IX_pipeline_results_JobApplicationId",
                table: "pipeline_results",
                newName: "IX_pipeline_results_job_application_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "job_applications",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "job_applications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ResumeText",
                table: "job_applications",
                newName: "resume_text");

            migrationBuilder.RenameColumn(
                name: "JobDescription",
                table: "job_applications",
                newName: "job_description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "job_applications",
                newName: "created_at");

            migrationBuilder.AddForeignKey(
                name: "FK_pipeline_results_job_applications_job_application_id",
                table: "pipeline_results",
                column: "job_application_id",
                principalTable: "job_applications",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pipeline_results_job_applications_job_application_id",
                table: "pipeline_results");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "pipeline_results",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "skill_extraction",
                table: "pipeline_results",
                newName: "SkillExtraction");

            migrationBuilder.RenameColumn(
                name: "resume_rewrite",
                table: "pipeline_results",
                newName: "ResumeRewrite");

            migrationBuilder.RenameColumn(
                name: "resume_match",
                table: "pipeline_results",
                newName: "ResumeMatch");

            migrationBuilder.RenameColumn(
                name: "job_application_id",
                table: "pipeline_results",
                newName: "JobApplicationId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "pipeline_results",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "cover_letter",
                table: "pipeline_results",
                newName: "CoverLetter");

            migrationBuilder.RenameIndex(
                name: "IX_pipeline_results_job_application_id",
                table: "pipeline_results",
                newName: "IX_pipeline_results_JobApplicationId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "job_applications",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "job_applications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "resume_text",
                table: "job_applications",
                newName: "ResumeText");

            migrationBuilder.RenameColumn(
                name: "job_description",
                table: "job_applications",
                newName: "JobDescription");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "job_applications",
                newName: "CreatedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_pipeline_results_job_applications_JobApplicationId",
                table: "pipeline_results",
                column: "JobApplicationId",
                principalTable: "job_applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
