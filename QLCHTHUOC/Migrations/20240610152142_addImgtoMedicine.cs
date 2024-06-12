using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLCHTHUOC.Migrations
{
    /// <inheritdoc />
    public partial class addImgtoMedicine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgURl",
                table: "Medicines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgURl",
                table: "Medicines");
        }
    }
}
