using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPath.Assessment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialAssessmentsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "assessments");

            migrationBuilder.CreateTable(
                name: "AssessmentSubmissions",
                schema: "assessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RawAnswersJson = table.Column<string>(type: "jsonb", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentResults",
                schema: "assessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentSubmissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PrimaryCareer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PrimaryConfidence = table.Column<decimal>(type: "numeric(5,4)", nullable: false),
                    SecondaryCareer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SecondaryConfidence = table.Column<decimal>(type: "numeric(5,4)", nullable: false),
                    TertiaryCareer = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TertiaryConfidence = table.Column<decimal>(type: "numeric(5,4)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentResults_AssessmentSubmissions_AssessmentSubmissio~",
                        column: x => x.AssessmentSubmissionId,
                        principalSchema: "assessments",
                        principalTable: "AssessmentSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResults_AssessmentSubmissionId",
                schema: "assessments",
                table: "AssessmentResults",
                column: "AssessmentSubmissionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentSubmissions_UserId",
                schema: "assessments",
                table: "AssessmentSubmissions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentResults",
                schema: "assessments");

            migrationBuilder.DropTable(
                name: "AssessmentSubmissions",
                schema: "assessments");
        }
    }
}
