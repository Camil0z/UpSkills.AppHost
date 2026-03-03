using Microsoft.EntityFrameworkCore;
using UpSkills.Infrastructure.Entidades;

namespace UpSkills.Infrastructure;

public class AppDbContext : DbContext
{ 
    public DbSet<Pais> Pais { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<Usuario> Usuario { get; set; }

    public AppDbContext (DbContextOptions dbOptions) : base(dbOptions)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pais>(b =>
        {
            b.ToTable(nameof(Pais), "public");
            b.HasKey(c => c.Id);
            b.HasIndex(c => c.Nombre).IsUnique();
        });

        modelBuilder.Entity<Rol>(b =>
        {
            b.ToTable(nameof(Rol), "users");
            b.HasKey(r => r.Id);
            b.HasIndex(r => r.Nombre).IsUnique();
        });

        modelBuilder.Entity<Usuario>(b =>
        {
            b.ToTable(nameof(Usuario), "users");
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.Correo).IsUnique();
            b.Property(u => u.CreadoEn)
                .HasColumnType("timestamp without time zone");
        });
    }
}
