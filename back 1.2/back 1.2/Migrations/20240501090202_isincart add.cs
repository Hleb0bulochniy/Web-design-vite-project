using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_1._2.Migrations
{
    /// <inheritdoc />
    public partial class isincartadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isInCart",
                table: "ItemsInUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isInCart",
                table: "ItemsInUser");
        }
    }
}
