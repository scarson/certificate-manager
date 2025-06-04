using Microsoft.EntityFrameworkCore;
using CertificateManager.Server.Models;

namespace CertificateManager.Server.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Certificate> Certificates { get; set; } = null!;
    public DbSet<SubjectAlternativeName> SubjectAlternativeNames { get; set; } = null!;

    // DbSet properties will be added here

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure your entities here
    }
}
