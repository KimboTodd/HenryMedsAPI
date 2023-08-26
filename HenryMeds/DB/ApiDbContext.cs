using Microsoft.EntityFrameworkCore;

namespace HenryMeds.DB;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    // Seed the db. TODO: move this to a separate file that is more production worthy
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Provider>().HasData(new Provider
            {
                Id = 1,
                Availabilities = new List<Availability>(),
            }
        );

        modelBuilder.Entity<Provider>(
            entity =>
            {
                entity.HasMany<Availability>(d => d.Availabilities)
                    .WithOne(d => d.Provider);
            });
    }

    // TODO: move connection string to somewhere safe
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=henry-meds;User Id=postgres;Password=postgres");
    }

    public DbSet<Availability>? Availabilities { get; set; }

    public DbSet<Provider>? Providers { get; set; }

    public DbSet<Client>? Clients { get; set; }

    public DbSet<Appointment>? Appointments { get; set; }
}