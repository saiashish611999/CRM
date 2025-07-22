using CRM.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace CRM.Domain;
public class CRMDbContext: DbContext
{
    public CRMDbContext(DbContextOptions<CRMDbContext> options): base(options)
    {
        
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Person> Persons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>().ToTable("tblCountries");
        modelBuilder.Entity<Person>().ToTable("tblPersons");

        // seed data
        string countriesJson = System.IO.File.ReadAllText("countries.json");
        List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
        countries!.ForEach(country => modelBuilder.Entity<Country>().HasData(country));

        string personsJson = System.IO.File.ReadAllText("persons.json");
        List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
        persons!.ForEach(person => modelBuilder.Entity<Person>().HasData(person));

        // FLUENT API
        modelBuilder.Entity<Person>().Property(prop => prop.TIN)
                                     .HasColumnName("TaxIdentificationNumber")
                                     .HasColumnType("varchar(8)")
                                     .HasDefaultValue("ABC12345");

        //modelBuilder.Entity<Person>().HasIndex(prop => prop.TIN)
        //                             .IsUnique();

        // modelBuilder.Entity<Person>().HasIndex(prop => prop.Country).IsUnique();

        modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN_Length", "Len([TaxIdentificationNumber]) = 8");

        // table relations
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasOne<Country>(country => country.Country)
            .WithMany(person => person.Persons)
            .HasForeignKey(person => person.CountryId);
        });
    }

    public List<Person> sp_GetAllPersons()
    {
        IQueryable<Person> persons =  Persons.FromSqlRaw("EXEC [dbo].[GetAllPersons]");
        return persons.ToList();
    }

    public int sp_InsertPerson(Person person)
    {
        SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@PersonId", person.PersonId),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email ?? (object)DBNull.Value),
        new SqlParameter("@Address", person.Address ?? (object)DBNull.Value),
        new SqlParameter("@CountryId", person.CountryId),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters),
        new SqlParameter("@Gender", person.Gender)};

        return Database.ExecuteSqlRaw("EXEC [dbo].[SP_InsertPerson] @PersonId, @PersonName, @Email, @Address, @CountryId, @DateOfBirth, @ReceiveNewsLetters, @Gender", parameters);
    }
}
