using Microsoft.EntityFrameworkCore;
using LoncotesLibrary.Models;

public class LoncotesLibraryDbContext : DbContext
{

    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Patron> Patrons { get; set; }

    public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
        {
        new MaterialType { Id = 1, Name = "Book", CheckoutDays = 14 },
        new MaterialType { Id = 2, Name = "Periodical", CheckoutDays = 7 },
        new MaterialType { Id = 3, Name = "CD", CheckoutDays = 10 },
        new MaterialType { Id = 4, Name = "DVD", CheckoutDays = 5 },
        new MaterialType { Id = 5, Name = "Audiobook", CheckoutDays = 14 }
        });

        modelBuilder.Entity<Genre>().HasData(new Genre[]
        {
        new Genre { Id = 1, Name = "Mystery" },
        new Genre { Id = 2, Name = "Science Fiction" },
        new Genre { Id = 3, Name = "Romance" },
        new Genre { Id = 4, Name = "History" },
        new Genre { Id = 5, Name = "Biography" },
        new Genre { Id = 6, Name = "Fantasy" },
        new Genre { Id = 7, Name = "Self-Help" }
        });

        modelBuilder.Entity<Patron>().HasData(new Patron[]
        {
        new Patron { Id = 1, FirstName = "Tim", LastName = "Compton", Address = "2266 East Eand Circle Portland TN 37456", Email = "tc@gmail.com", IsActive = true },
        new Patron { Id = 2, FirstName = "Sarah", LastName = "Johnson", Address = "1523 Oak Street Nashville TN 37203", Email = "sarah.j@email.com", IsActive = true },
        new Patron { Id = 3, FirstName = "Marcus", LastName = "Williams", Address = "789 Maple Ave Hendersonville TN 37075", Email = "mwilliams@email.com", IsActive = true },
        new Patron { Id = 4, FirstName = "Emily", LastName = "Davis", Address = "456 Pine Road Franklin TN 37064", Email = "emily.davis@email.com", IsActive = false }
        });

        modelBuilder.Entity<Material>().HasData(new Material[]
        {
        new Material { Id = 1, MaterialName = "The Hound of the Baskervilles", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null },
        new Material { Id = 2, MaterialName = "Dune", MaterialTypeId = 1, GenreId = 2, OutOfCirculationSince = null },
        new Material { Id = 3, MaterialName = "Pride and Prejudice", MaterialTypeId = 1, GenreId = 3, OutOfCirculationSince = null },
        new Material { Id = 4, MaterialName = "The Guns of August", MaterialTypeId = 1, GenreId = 4, OutOfCirculationSince = null },
        new Material { Id = 5, MaterialName = "Steve Jobs Biography", MaterialTypeId = 1, GenreId = 5, OutOfCirculationSince = null },
        new Material { Id = 6, MaterialName = "The Hobbit", MaterialTypeId = 1, GenreId = 6, OutOfCirculationSince = null },
        new Material { Id = 7, MaterialName = "National Geographic - October 2024", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = null },
        new Material { Id = 8, MaterialName = "The Best of Mozart", MaterialTypeId = 3, GenreId = 4, OutOfCirculationSince = null },
        new Material { Id = 9, MaterialName = "Inception", MaterialTypeId = 4, GenreId = 2, OutOfCirculationSince = null },
        new Material { Id = 10, MaterialName = "Atomic Habits", MaterialTypeId = 5, GenreId = 7, OutOfCirculationSince = null },
        new Material { Id = 11, MaterialName = "Murder on the Orient Express", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = new DateTime(2023, 5, 15) },
        new Material { Id = 12, MaterialName = "The Fellowship of the Ring", MaterialTypeId = 1, GenreId = 6, OutOfCirculationSince = null },
        new Material { Id = 13, MaterialName = "Beatles Greatest Hits", MaterialTypeId = 3, GenreId = 4, OutOfCirculationSince = null },
        new Material { Id = 14, MaterialName = "The Matrix", MaterialTypeId = 4, GenreId = 2, OutOfCirculationSince = null },
        new Material { Id = 15, MaterialName = "Becoming by Michelle Obama", MaterialTypeId = 5, GenreId = 5, OutOfCirculationSince = null }
        });

        modelBuilder.Entity<Checkout>().HasData(new Checkout[]
        {
        new Checkout { Id = 1, MaterialId = 1, PatronId = 1, CheckoutDate = new DateTime(2024, 9, 1), ReturnDate = new DateTime(2024, 9, 10) },
        new Checkout { Id = 2, MaterialId = 2, PatronId = 2, CheckoutDate = new DateTime(2024, 9, 15), ReturnDate = new DateTime(2024, 9, 28) },
        new Checkout { Id = 3, MaterialId = 6, PatronId = 1, CheckoutDate = new DateTime(2024, 9, 20), ReturnDate = null },  // Still checked out
        new Checkout { Id = 4, MaterialId = 9, PatronId = 3, CheckoutDate = new DateTime(2024, 9, 25), ReturnDate = null },  // Still checked out
        new Checkout { Id = 5, MaterialId = 10, PatronId = 2, CheckoutDate = new DateTime(2024, 8, 10), ReturnDate = new DateTime(2024, 8, 22) }
        });
    }
}