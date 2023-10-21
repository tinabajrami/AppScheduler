using AppScheduler_v1.Areas.Identity.Data;
using AppScheduler_v1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppScheduler_v1.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.PatientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany() // Assuming a doctor doesn't have a navigation property back to appointments
            .HasForeignKey(a => a.DoctorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ApplyConfiguration(new AppUserEntityConfiguration());
    }

    public DbSet<Appointment> Appointments { get; set; }
}

public class AppUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(255);
        builder.Property(u => u.LastName).HasMaxLength(255);
    }
}

