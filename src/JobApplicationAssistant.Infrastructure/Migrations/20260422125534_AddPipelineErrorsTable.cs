using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobApplicationAssistant.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPipelineErrorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "pipeline_errors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    job_application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    failed_step = table.Column<string>(type: "text", nullable: false),
                    error_message = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pipeline_errors", x => x.id);
                    table.ForeignKey(
                        name: "FK_pipeline_errors_job_applications_job_application_id",
                        column: x => x.job_application_id,
                        principalTable: "job_applications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pipeline_errors_job_application_id",
                table: "pipeline_errors",
                column: "job_application_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pipeline_errors");
        }
    }
}
