using Microsoft.EntityFrameworkCore;
using Shared;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<PriceList> PriceList { get; set; }
    public required DbSet<Leg> Legs { get; set; }
    public required DbSet<RouteInfo> RouteInfos { get; set; }
    public required DbSet<Location> Locations { get; set; }
    public required DbSet<Provider> Providers { get; set; }
    public required DbSet<Company> Companies { get; set; }
    public required DbSet<Reservation> Reservations { get; set; }
    public required DbSet<ReservationLeg> ReservationLegs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Leg>()
            .HasMany(l => l.Providers)
            .WithOne()
            .HasForeignKey("LegId");

        modelBuilder.Entity<Provider>()
            .HasOne(p => p.Company)
            .WithMany()
            .HasForeignKey("CompanyId");
    }
}