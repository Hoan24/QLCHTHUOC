using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHTHUOC.Migrations
{
    /// <inheritdoc />
    public partial class addImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "Medicines",
                newName: "ImgURl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgURl",
                table: "Medicines",
                newName: "ImageFileName");
        }
    }
}
