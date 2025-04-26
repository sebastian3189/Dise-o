using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM_ITM.Migrations
{
    /// <inheritdoc />
    public partial class CambioDos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
        name: "UQ__Horarios__CA4C0888EC82DF7A",
        table: "Horarios");

        }
    }
}
