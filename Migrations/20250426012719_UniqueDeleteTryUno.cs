using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM_ITM.Migrations
{
    /// <inheritdoc />
    public partial class UniqueDeleteTryUno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropUniqueConstraint(
             name: "UQ__Horarios__CA4C0888EC82DF7A",
             table: "Horarios");

            //migrationBuilder.DropIndex(
            //    name: "UQ__Horarios__CA4C0888EC82DF7A",
            //    table: "Horarios");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
