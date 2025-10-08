# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

LoncotesLibrary is a library management system built with ASP.NET Core Web API and Entity Framework Core, using PostgreSQL as the database. The application manages library materials (books, periodicals, CDs, DVDs, audiobooks), patrons, and checkouts.

## Common Commands

### Running the Application
```bash
dotnet run
```

### Database Management
```bash
# Create a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations to database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

### Building and Cleaning
```bash
# Build the project
dotnet build

# Clean build artifacts
dotnet clean
```

## Architecture

### Data Model
The application uses a relational data model with the following core entities:

- **Material**: Library items with MaterialTypeId and GenreId foreign keys. Tracks OutOfCirculationSince for items no longer available
- **MaterialType**: Defines types (Book, Periodical, CD, DVD, Audiobook) with associated CheckoutDays
- **Genre**: Categories for materials (Mystery, Science Fiction, Romance, History, Biography, Fantasy, Self-Help)
- **Patron**: Library users with IsActive flag to track account status
- **Checkout**: Junction entity linking Materials and Patrons, with CheckoutDate and ReturnDate (null when still checked out)

### Database Context
`LoncotesLibraryDbContext` contains seed data for all entities. The seed data includes:
- 5 material types with varying checkout periods
- 7 genres
- 4 patrons (3 active, 1 inactive)
- 15 materials (one out of circulation)
- 5 checkouts (2 still checked out)

### DTO Pattern
DTOs are located in `Models/DTO/` directory and exclude navigation properties to prevent circular references during serialization.

## Configuration

### Database Connection
The PostgreSQL connection string is configured via user secrets with key `LoncotesLibraryDbConnectionString`. The connection is registered in Program.cs with legacy timestamp behavior enabled for Npgsql.

### Technology Stack
- .NET 9.0
- Entity Framework Core 8.0
- Npgsql.EntityFrameworkCore.PostgreSQL 8.0
- OpenAPI/Swagger (enabled in development)
