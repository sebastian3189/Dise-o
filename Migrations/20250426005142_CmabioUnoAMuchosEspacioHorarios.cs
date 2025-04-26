using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM_ITM.Migrations
{
    /// <inheritdoc />
    public partial class CmabioUnoAMuchosEspacioHorarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_IdEspacio",
                table: "Horarios",
                column: "IdEspacio",
                unique: true,
                filter: "[IdEspacio] IS NOT NULL");
        }
    }
}
