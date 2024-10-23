using HotDeskBooking.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.Models.Entity;

public class HotDeskBookingContext: DbContext
{
    public HotDeskBookingContext()
    {
    }

    public HotDeskBookingContext(DbContextOptions<HotDeskBookingContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            var builder = WebApplication.CreateBuilder();
            var connectionString = builder.Configuration.GetConnectionString("DbContext");

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            
            entity.HasKey(e => e.Id);
            
            entity.Property(x => x.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("locations");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);
            entity.Property(x => x.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Desk>(entity =>
        {
            entity.ToTable("desks");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity.Property(x => x.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);
            entity.Property(x => x.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.ToTable("user_refresh_tokens");

            entity.HasKey("UserId");

            entity.Property(e => e.RefreshTokenExpires)
                .HasColumnType("datetime")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasData(
                new UserRole() { Id = 1, Type = UserTypes.Admin.ToString() },
                new UserRole() { Id = 2, Type = UserTypes.Employee.ToString() }
            );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasData(
                new User() { Id = 1, Email = "admin@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"), RoleId = 1 }
            );
        });
    }
}
