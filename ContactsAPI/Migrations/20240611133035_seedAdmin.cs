using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LocalUsers",
                columns: new[] { "Id", "Name", "Passsword", "Role", "UserName" },
                values: new object[] { 1, "admin", "Admin1", "admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocalUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
