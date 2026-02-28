using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    FullName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    FamilyGroupId = table.Column<int>(type: "int", nullable: true),
                    FamilyGroupOwnerId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_FamilyGroups_FamilyGroupId",
                        column: x => x.FamilyGroupId,
                        principalTable: "FamilyGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_FamilyGroups_FamilyGroupOwnerId",
                        column: x => x.FamilyGroupOwnerId,
                        principalTable: "FamilyGroups",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FamilyGroupId",
                table: "Users",
                column: "FamilyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FamilyGroupOwnerId",
                table: "Users",
                column: "FamilyGroupOwnerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupOwnerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupId",
                table: "AspNetUsers",
                column: "FamilyGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupOwnerId",
                table: "AspNetUsers",
                column: "FamilyGroupOwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupId",
                table: "AspNetUsers",
                column: "FamilyGroupId",
                principalTable: "FamilyGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupOwnerId",
                table: "AspNetUsers",
                column: "FamilyGroupOwnerId",
                principalTable: "FamilyGroups",
                principalColumn: "Id");
        }
    }
}
