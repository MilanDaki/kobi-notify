using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kobi_notify.Migrations
{
    /// <inheritdoc />
    public partial class AddFallbackRuleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FallbackRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    FallbackType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FallbackValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FallbackRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FallbackRules_CustomerProfiles_CustomerProfileId",
                        column: x => x.CustomerProfileId,
                        principalTable: "CustomerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FallbackRules_CustomerProfileId",
                table: "FallbackRules",
                column: "CustomerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FallbackRules");
        }
    }
}
