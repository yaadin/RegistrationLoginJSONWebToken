using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoginWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_heroes",
                table: "heroes");

            migrationBuilder.RenameTable(
                name: "heroes",
                newName: "users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "heroes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_heroes",
                table: "heroes",
                column: "Id");
        }
    }
}
