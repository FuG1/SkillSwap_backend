using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace skillswap.Migrations
{
    /// <inheritdoc />
    public partial class asdasdasdsadf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "users",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "users");
        }
    }
}
