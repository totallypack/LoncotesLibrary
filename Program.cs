using LoncotesLibrary.Models;
using LoncotesLibrary.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDbConnectionString"]);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:5053")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

// Get all circulating Materials with Genre and MaterialType
// Optional query parameters: materialTypeId, genreId
app.MapGet("/api/materials", (LoncotesLibraryDbContext db, int? materialTypeId, int? genreId) =>
{
    var materials = db.Materials
        .Include(m => m.Genre)
        .Include(m => m.MaterialType)
        .Where(m => m.OutOfCirculationSince == null);

    if (materialTypeId.HasValue)
    {
        materials = materials.Where(m => m.MaterialTypeId == materialTypeId.Value);
    }

    if (genreId.HasValue)
    {
        materials = materials.Where(m => m.GenreId == genreId.Value);
    }

    return materials.Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        GenreId = m.GenreId,
        OutOfCirculationSince = m.OutOfCirculationSince,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        }
    }).ToList();
});

// Get Material by Id with Genre, MaterialType, and Checkouts (with Patrons)
app.MapGet("/api/materials/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    var material = db.Materials
        .Include(m => m.Genre)
        .Include(m => m.MaterialType)
        .Include(m => m.Checkouts)
            .ThenInclude(c => c.Patron)
        .FirstOrDefault(m => m.Id == id);

    if (material == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince,
        MaterialType = new MaterialTypeDTO
        {
            Id = material.MaterialType.Id,
            Name = material.MaterialType.Name,
            CheckoutDays = material.MaterialType.CheckoutDays
        },
        Genre = new GenreDTO
        {
            Id = material.Genre.Id,
            Name = material.Genre.Name
        },
        Checkouts = material.Checkouts.Select(c => new CheckoutDTO
        {
            Id = c.Id,
            MaterialId = c.MaterialId,
            PatronId = c.PatronId,
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate,
            Patron = new PatronDTO
            {
                Id = c.Patron.Id,
                FirstName = c.Patron.FirstName,
                LastName = c.Patron.LastName,
                Address = c.Patron.Address,
                Email = c.Patron.Email,
                IsActive = c.Patron.IsActive
            }
        }).ToList()
    });
});

// Add a Material
app.MapPost("/api/materials", (LoncotesLibraryDbContext db, Material material) =>
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

// Remove a Material from Circulation (soft delete)
app.MapPut("/api/materials/{id}/remove", (LoncotesLibraryDbContext db, int id) =>
{
    var material = db.Materials.FirstOrDefault(m => m.Id == id);
    if (material == null)
    {
        return Results.NotFound();
    }

    material.OutOfCirculationSince = DateTime.Now;
    db.SaveChanges();
    return Results.NoContent();
});

// Get all MaterialTypes
app.MapGet("/api/materialtypes", (LoncotesLibraryDbContext db) =>
{
    return db.MaterialTypes.Select(mt => new MaterialTypeDTO
    {
        Id = mt.Id,
        Name = mt.Name,
        CheckoutDays = mt.CheckoutDays
    }).ToList();
});

// Get all Genres
app.MapGet("/api/genres", (LoncotesLibraryDbContext db) =>
{
    return db.Genres.Select(g => new GenreDTO
    {
        Id = g.Id,
        Name = g.Name
    }).ToList();
});

// Get all Patrons
app.MapGet("/api/patrons", (LoncotesLibraryDbContext db) =>
{
    return db.Patrons.Select(p => new PatronDTO
    {
        Id = p.Id,
        FirstName = p.FirstName,
        LastName = p.LastName,
        Address = p.Address,
        Email = p.Email,
        IsActive = p.IsActive
    }).ToList();
});

// Get Patron by Id with Checkouts (including Materials and MaterialTypes)
app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    var patron = db.Patrons
        .Include(p => p.Checkouts)
            .ThenInclude(c => c.Material)
                .ThenInclude(m => m.MaterialType)
        .FirstOrDefault(p => p.Id == id);

    if (patron == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new PatronDTO
    {
        Id = patron.Id,
        FirstName = patron.FirstName,
        LastName = patron.LastName,
        Address = patron.Address,
        Email = patron.Email,
        IsActive = patron.IsActive,
        Checkouts = patron.Checkouts.Select(c => new CheckoutDTO
        {
            Id = c.Id,
            MaterialId = c.MaterialId,
            PatronId = c.PatronId,
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate,
            Material = new MaterialDTO
            {
                Id = c.Material.Id,
                MaterialName = c.Material.MaterialName,
                MaterialTypeId = c.Material.MaterialTypeId,
                GenreId = c.Material.GenreId,
                OutOfCirculationSince = c.Material.OutOfCirculationSince,
                MaterialType = new MaterialTypeDTO
                {
                    Id = c.Material.MaterialType.Id,
                    Name = c.Material.MaterialType.Name,
                    CheckoutDays = c.Material.MaterialType.CheckoutDays
                }
            }
        }).ToList()
    });
});

// Update Patron (Address and Email only)
app.MapPut("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id, Patron patron) =>
{
    var patronToUpdate = db.Patrons.FirstOrDefault(p => p.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }

    patronToUpdate.Address = patron.Address;
    patronToUpdate.Email = patron.Email;
    db.SaveChanges();
    return Results.NoContent();
});

// Deactivate Patron (soft delete)
app.MapPut("/api/patrons/{id}/deactivate", (LoncotesLibraryDbContext db, int id) =>
{
    var patron = db.Patrons.FirstOrDefault(p => p.Id == id);
    if (patron == null)
    {
        return Results.NotFound();
    }

    patron.IsActive = false;
    db.SaveChanges();
    return Results.NoContent();
});

// Get All Checkouts with Material and Patron
app.MapGet("/api/checkouts", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
        .Include(c => c.Material)
            .ThenInclude(m => m.MaterialType)
        .Include(c => c.Material.Genre)
        .Include(c => c.Patron)
        .OrderByDescending(c => c.CheckoutDate)
        .Select(c => new CheckoutDTO
        {
            Id = c.Id,
            MaterialId = c.MaterialId,
            PatronId = c.PatronId,
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate,
            Material = new MaterialDTO
            {
                Id = c.Material.Id,
                MaterialName = c.Material.MaterialName,
                MaterialTypeId = c.Material.MaterialTypeId,
                GenreId = c.Material.GenreId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = c.Material.MaterialType.Id,
                    Name = c.Material.MaterialType.Name,
                    CheckoutDays = c.Material.MaterialType.CheckoutDays
                },
                Genre = new GenreDTO
                {
                    Id = c.Material.Genre.Id,
                    Name = c.Material.Genre.Name
                }
            },
            Patron = new PatronDTO
            {
                Id = c.Patron.Id,
                FirstName = c.Patron.FirstName,
                LastName = c.Patron.LastName,
                Email = c.Patron.Email,
                Address = c.Patron.Address,
                IsActive = c.Patron.IsActive
            }
        }).ToList();
});

// Checkout a Material
app.MapPost("/api/checkouts", (LoncotesLibraryDbContext db, Checkout checkout) =>
{
    checkout.CheckoutDate = DateTime.Today;
    db.Checkouts.Add(checkout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{checkout.Id}", checkout);
});

// Return a Material
app.MapPut("/api/checkouts/{id}/return", (LoncotesLibraryDbContext db, int id) =>
{
    var checkout = db.Checkouts.FirstOrDefault(c => c.Id == id);
    if (checkout == null)
    {
        return Results.NotFound();
    }

    checkout.ReturnDate = DateTime.Today;
    db.SaveChanges();
    return Results.NoContent();
});

// Get Only Available Materials
app.MapGet("/api/materials/available", (LoncotesLibraryDbContext db) =>
{
    return db.Materials
    .Include(m => m.MaterialType)
    .Include(m => m.Genre)
    .Where(m => m.OutOfCirculationSince == null)
    .Where(m => m.Checkouts.All(co => co.ReturnDate != null))
    .Select(material => new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince,
        MaterialType = new MaterialTypeDTO
        {
            Id = material.MaterialType.Id,
            Name = material.MaterialType.Name,
            CheckoutDays = material.MaterialType.CheckoutDays
        },
        Genre = new GenreDTO
        {
            Id = material.Genre.Id,
            Name = material.Genre.Name
        }
    })
    .ToList();
});

// Get Overdue Checkouts
app.MapGet("/api/checkouts/overdue", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
    .Include(p => p.Patron)
    .Include(co => co.Material)
    .ThenInclude(m => m.MaterialType)
    .Where(co =>
        (DateTime.Today - co.CheckoutDate).Days >
        co.Material.MaterialType.CheckoutDays &&
        co.ReturnDate == null)
        .Select(co => new CheckoutDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                MaterialName = co.Material.MaterialName,
                MaterialTypeId = co.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = co.Material.MaterialTypeId,
                    Name = co.Material.MaterialType.Name,
                    CheckoutDays = co.Material.MaterialType.CheckoutDays
                },
                GenreId = co.Material.GenreId,
                OutOfCirculationSince = co.Material.OutOfCirculationSince
            },
            PatronId = co.PatronId,
            Patron = new PatronDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate
        })
    .ToList();
});

app.Run();