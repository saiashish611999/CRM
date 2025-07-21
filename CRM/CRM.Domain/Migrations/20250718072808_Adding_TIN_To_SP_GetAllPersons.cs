using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Adding_TIN_To_SP_GetAllPersons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"ALTER PROCEDURE [dbo].[GetAllPersons]
                                        AS BEGIN
                                             SELECT PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters, TIN
                                             FROM [dbo].[tblPersons]
                                        END";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"ALTER PROCEDURE [dbo].[GetAllPersons]
                                        AS BEGIN
                                             SELECT PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters
                                             FROM [dbo].[tblPersons]
                                        END";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
