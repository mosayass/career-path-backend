using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPath.Assessment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCareerStringsWithJobLabels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryCareer",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.DropColumn(
                name: "SecondaryCareer",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.DropColumn(
                name: "TertiaryCareer",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TertiaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.DropColumn(
                name: "SecondaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.DropColumn(
                name: "TertiaryJobLabel",
                schema: "assessments",
                table: "AssessmentResults");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCareer",
                schema: "assessments",
                table: "AssessmentResults",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondaryCareer",
                schema: "assessments",
                table: "AssessmentResults",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TertiaryCareer",
                schema: "assessments",
                table: "AssessmentResults",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
