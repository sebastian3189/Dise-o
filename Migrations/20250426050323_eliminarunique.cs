using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM_ITM.Migrations
{
    /// <inheritdoc />
    public partial class eliminarunique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
        name: "UQ__Horarios__CA4C08884301320E",
        table: "Horarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
