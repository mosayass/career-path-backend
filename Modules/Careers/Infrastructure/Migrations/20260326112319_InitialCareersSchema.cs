using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPath.Careers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCareersSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "careers");

            migrationBuilder.CreateTable(
                name: "CareerSectors",
                schema: "careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerSectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Careers",
                schema: "careers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AiLabelId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    EducationLevel = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CoreSkills = table.Column<string>(type: "jsonb", nullable: false),
                    SectorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Careers_CareerSectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "careers",
                        principalTable: "CareerSectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Careers_AiLabelId",
                schema: "careers",
                table: "Careers",
                column: "AiLabelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Careers_SectorId",
                schema: "careers",
                table: "Careers",
                column: "SectorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Careers",
                schema: "careers");

            migrationBuilder.DropTable(
                name: "CareerSectors",
                schema: "careers");
        }
    }
}
