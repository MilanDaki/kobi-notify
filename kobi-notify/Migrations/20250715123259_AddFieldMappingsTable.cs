using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kobi_notify.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldMappingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshInterval",
                table: "CustomerProfiles",
                newName: "RefreshIntervalMinutes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshIntervalMinutes",
                table: "CustomerProfiles",
                newName: "RefreshInterval");
        }
    }
}
