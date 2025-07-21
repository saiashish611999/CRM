using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Domain.Migrations
{
    /// <inheritdoc />
    public partial class GetPersons_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"CREATE PROCEDURE [dbo].[GetAllPersons]
                                        AS BEGIN
                                             SELECT PersonId, PersonName, Email, DateOfBirth, Gender, CountryId, Address, ReceiveNewsLetters
                                             FROM [dbo].[tblPersons]
                                        END";
            migrationBuilder.Sql(sp_GetAllPersons);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllPersons]";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
