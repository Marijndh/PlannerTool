using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlannerTool.Migrations
{
    /// <inheritdoc />
    public partial class DeadlineAndState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deadline",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "ProjectTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ProjectTasks");
        }
    }
}
