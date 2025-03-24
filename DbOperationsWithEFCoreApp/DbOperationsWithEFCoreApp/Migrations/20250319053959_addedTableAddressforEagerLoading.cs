using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbOperationsWithEFCoreApp.Migrations
{
    /// <inheritdoc />
    public partial class addedTableAddressforEagerLoading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Addressid",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_Addressid",
                table: "Authors",
                column: "Addressid");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Addresses_Addressid",
                table: "Authors",
                column: "Addressid",
                principalTable: "Addresses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Addresses_Addressid",
                table: "Authors");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Authors_Addressid",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Addressid",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Authors");
        }
    }
}
