using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityApp.Model.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGroupAndTeacher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Groups_GroupId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_GroupId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Teachers");

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Teachers_TeacherId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Teachers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_GroupId",
                table: "Teachers",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Groups_GroupId",
                table: "Teachers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
