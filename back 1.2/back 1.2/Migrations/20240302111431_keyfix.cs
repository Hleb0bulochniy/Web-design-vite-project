using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_1._2.Migrations
{
    /// <inheritdoc />
    public partial class keyfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInUsers_AddressInUsers_AddressInUserId",
                table: "AddressInUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressInUsers_Users_userid",
                table: "AddressInUsers");

            migrationBuilder.DropIndex(
                name: "IX_AddressInUsers_AddressInUserId",
                table: "AddressInUsers");

            migrationBuilder.DropColumn(
                name: "AddressInUserId",
                table: "AddressInUsers");

            migrationBuilder.RenameColumn(
                name: "userid",
                table: "AddressInUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "addressid",
                table: "AddressInUsers",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_AddressInUsers_userid",
                table: "AddressInUsers",
                newName: "IX_AddressInUsers_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressInUsers_AddressId",
                table: "AddressInUsers",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInUsers_Address_AddressId",
                table: "AddressInUsers",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInUsers_Users_UserId",
                table: "AddressInUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInUsers_Address_AddressId",
                table: "AddressInUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AddressInUsers_Users_UserId",
                table: "AddressInUsers");

            migrationBuilder.DropIndex(
                name: "IX_AddressInUsers_AddressId",
                table: "AddressInUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AddressInUsers",
                newName: "userid");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "AddressInUsers",
                newName: "addressid");

            migrationBuilder.RenameIndex(
                name: "IX_AddressInUsers_UserId",
                table: "AddressInUsers",
                newName: "IX_AddressInUsers_userid");

            migrationBuilder.AddColumn<int>(
                name: "AddressInUserId",
                table: "AddressInUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressInUsers_AddressInUserId",
                table: "AddressInUsers",
                column: "AddressInUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInUsers_AddressInUsers_AddressInUserId",
                table: "AddressInUsers",
                column: "AddressInUserId",
                principalTable: "AddressInUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInUsers_Users_userid",
                table: "AddressInUsers",
                column: "userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
