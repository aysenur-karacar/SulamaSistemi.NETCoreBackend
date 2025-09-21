using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SulamaSistemiWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMotorRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastOperationByUserId",
                table: "Motors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motors_LastOperationByUserId",
                table: "Motors",
                column: "LastOperationByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Motors_Users_LastOperationByUserId",
                table: "Motors",
                column: "LastOperationByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motors_Users_LastOperationByUserId",
                table: "Motors");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Motors_LastOperationByUserId",
                table: "Motors");

            migrationBuilder.DropColumn(
                name: "LastOperationByUserId",
                table: "Motors");
        }
    }
}
