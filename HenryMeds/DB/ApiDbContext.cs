using Microsoft.EntityFrameworkCore;

namespace HenryMeds.DB;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=henry-meds;User Id=postgres;Password=postgres");

    public DbSet<Availability>? Availabilities { get; set; }

    public DbSet<Provider>? Providers { get; set; }
}