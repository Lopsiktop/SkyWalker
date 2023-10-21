using Microsoft.EntityFrameworkCore;
using SkyWalker.Models;

namespace SkyWalker.Data;

public class SkyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Station> Stations { get; set; }

    public DbSet<Transport> Transports { get; set; }

    public DbSet<Rent> Rents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SkyWalkerDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Transport>()
            .HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .IsRequired();

        mb.Entity<Transport>()
            .HasOne(x => x.Station)
            .WithMany()
            .HasForeignKey(x => x.StationId)
            .IsRequired();

        mb.Entity<Rent>()
            .HasOne(x => x.Transport)
            .WithMany()
            .HasForeignKey(x => x.TransportId)
            .IsRequired();

        mb.Entity<Rent>()
            .HasOne(x => x.Renter)
            .WithMany()
            .HasForeignKey(x => x.RenterId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(mb);
    }
}