using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InsertPerson_StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"CREATE PROCEDURE [dbo].[SP_InsertPerson]
                                       (@PersonId uniqueidentifier,
                                        @PersonName nvarchar(max),
                                        @Email nvarchar(max),
                                        @Address nvarchar(max),
                                        @CountryId uniqueidentifier,
                                        @DateOfBirth datetime2(7),
                                        @ReceiveNewsLetters bit,
                                        @Gender nvarchar(max))
                                            AS Begin
                                               INSERT INTO [dbo].[tblPersons]
                                               (PersonId, PersonName, Email, Address, CountryId, DateOfBirth, ReceiveNewsLetters, Gender)
                                               VALUES
                                                (@PersonId, @PersonName, @Email, @Address, @CountryId, @DateOfBirth, @ReceiveNewsLetters, @Gender)
                                            END";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"DROP PROCEDURE [dbo].[SP_InsertPerson]";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
