### Differences between TPH and TPT in Entity Framework Core

The difference between TPH (Table Per Hierarchy) and TPT (Table Per Type) in Entity Framework Core lies in how the data of derived classes are stored in the database.

### TPH (Table Per Hierarchy)

**Table Per Hierarchy** stores all derived classes in a single table. A discriminator column is used to differentiate which type of entity each row represents.

#### Advantages:
- Simplicity in the database structure.
- Faster queries in smaller tables, as no joins are required.

#### Disadvantages:
- The table can grow quickly if there are many derived classes or many specific columns for each type.
- Waste of space due to columns not used by all derived classes.

#### Example:

```csharp
public abstract class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; } // Social Security Number for Individual and EIN for Business
}

public class Individual : Person
{
    public string SocialSecurityNumber { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Business : Person
{
    public string EIN { get; set; }
    public DateTime IncorporationDate { get; set; }
}

public class MyDbContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Individual>("Individual")
            .HasValue<Business>("Business");
    }
}
```

### TPT (Table Per Type)

**Table Per Type** stores each derived class in its own table. Each derived table contains a foreign key referencing the base table.

#### Advantages:
- Better data normalization.
- Avoids null columns for types that do not use certain properties.

#### Disadvantages:
- Queries can be slower due to the joins required to reconstruct the complete entity.
- More complex database structure.

#### Example:

```csharp
public abstract class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; } // Social Security Number for Individual and EIN for Business
}

public class Individual : Person
{
    public string SocialSecurityNumber { get; set; }
    public DateTime BirthDate { get; set; }
}

public class Business : Person
{
    public string EIN { get; set; }
    public DateTime IncorporationDate { get; set; }
}

public class MyDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<Business> Businesses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_pw");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("People");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Document).IsRequired().HasMaxLength(20);
        });

        modelBuilder.Entity<Individual>(entity =>
        {
            entity.ToTable("Individuals");
            entity.Property(e => e.SocialSecurityNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.BirthDate).IsRequired();
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.ToTable("Businesses");
            entity.Property(e => e.EIN).IsRequired().HasMaxLength(20);
            entity.Property(e => e.IncorporationDate).IsRequired();
        });
    }
}
```

### Summary of Differences:

- **TPH**: Uses a single table to store all derived classes. A discriminator column differentiates the types.
  - **Pros**: Simplicity and performance in simple queries.
  - **Cons**: Potentially large table and wasted space.

- **TPT**: Uses separate tables for each derived class. Derived tables have foreign keys to the base table.
  - **Pros**: Better normalization, no null columns.
  - **Cons**: More complex and potentially slower queries due to required joins.
