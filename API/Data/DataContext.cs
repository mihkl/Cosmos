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

         modelBuilder.Entity<Leg>()
            .HasOne(l => l.RouteInfo)
            .WithOne()
            .HasForeignKey<Leg>("RouteInfoId");

        modelBuilder.Entity<PriceList>()
            .HasMany(p => p.Legs)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Leg>()
            .HasMany(l => l.Providers)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Leg>()
            .HasOne(l => l.RouteInfo)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RouteInfo>()
            .HasOne(r => r.From)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RouteInfo>()
            .HasOne(r => r.To)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PriceList>()
            .HasMany(p => p.Reservations)
            .WithOne()
            .HasForeignKey("PriceListId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasMany(r => r.ReservationLegs)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReservationLeg>()
            .HasOne(rl => rl.Leg)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReservationLeg>()
            .HasOne(rl => rl.Provider)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}