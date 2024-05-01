using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_1._2.Migrations
{
    /// <inheritdoc />
    public partial class itemincartnumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "itemInCartNumber",
                table: "ItemsInUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "itemInCartNumber",
                table: "ItemsInUser");
        }
    }
}
