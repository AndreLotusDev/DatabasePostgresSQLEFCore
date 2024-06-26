using DatabasePostgresSQLEFCore.Helper;
using DatabasePostgresSQLEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabasePostgresSQLEFCore
{
    public class ApplicationDbContext  : DbContext
    {
        private readonly SortedIdGenerator _sortedIdGenerator;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, SortedIdGenerator sortedIdGenerator)
           : base(options)
        {
            _sortedIdGenerator = sortedIdGenerator;
        }

        public DbSet<People> People { get; set; }
        public DbSet<PeopleLegal> PeopleLegal { get; set; }
        public DbSet<PeoplePhysical> PeoplePhysical { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<People>(entity =>
            {
                entity.ToTable("people");
                entity.HasKey(e => e.Id);
            });

            builder.Entity<PeopleLegal>(entity =>
            {
                entity.ToTable("people_legal");
            });

            builder.Entity<PeoplePhysical>(entity =>
            {
                entity.ToTable("people_physical");
            });

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.Id = _sortedIdGenerator.GenerateId();
                }
            }

            return base.SaveChanges();
        }
    }
}
