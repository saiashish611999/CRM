using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TIN_CHECK_CONSTRAINT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN_Length",
                table: "tblPersons",
                sql: "Len([TaxIdentificationNumber]) = 8");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN_Length",
                table: "tblPersons");
        }
    }
}
