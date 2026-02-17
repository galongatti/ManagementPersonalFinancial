using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserFamilyGroupOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FamilyGroupOwnerId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyGroupOwnerId",
                table: "AspNetUsers",
                column: "FamilyGroupOwnerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupOwnerId",
                table: "AspNetUsers",
                column: "FamilyGroupOwnerId",
                principalTable: "FamilyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FamilyGroups_FamilyGroupOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyGroupOwnerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyGroupOwnerId",
                table: "AspNetUsers");
        }
    }
}
