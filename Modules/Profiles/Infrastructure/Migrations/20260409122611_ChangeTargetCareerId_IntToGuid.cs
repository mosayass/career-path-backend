using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPath.Profiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTargetCareerId_IntToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop and recreate the column since int cannot be directly cast to uuid
            migrationBuilder.DropColumn(
                name: "TargetCareerId",
                schema: "profiles",
                table: "UserProfiles");

            migrationBuilder.AddColumn<Guid>(
                name: "TargetCareerId",
                schema: "profiles",
                table: "UserProfiles",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TargetCareerId",
                schema: "profiles",
                table: "UserProfiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
