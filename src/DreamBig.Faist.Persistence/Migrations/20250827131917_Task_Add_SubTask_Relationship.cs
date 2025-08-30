using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamBig.Faist.Persistence.Migrations;

/// <inheritdoc />
public partial class Task_Add_SubTask_Relationship : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ParentTaskId",
            table: "Tasks",
            type: "uuid",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tasks_ParentTaskId",
            table: "Tasks",
            column: "ParentTaskId");

        migrationBuilder.AddForeignKey(
            name: "FK_Tasks_Tasks_ParentTaskId",
            table: "Tasks",
            column: "ParentTaskId",
            principalTable: "Tasks",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Tasks_Tasks_ParentTaskId",
            table: "Tasks");

        migrationBuilder.DropIndex(
            name: "IX_Tasks_ParentTaskId",
            table: "Tasks");

        migrationBuilder.DropColumn(
            name: "ParentTaskId",
            table: "Tasks");
    }
}
