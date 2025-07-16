using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kobi_notify.Migrations
{
    /// <inheritdoc />
    public partial class AddModelTypeToDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModelType",
                table: "CustomerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModelType",
                table: "CustomerProfiles");
        }
    }
}
