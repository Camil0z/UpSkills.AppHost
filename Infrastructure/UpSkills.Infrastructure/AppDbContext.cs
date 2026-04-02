using Microsoft.EntityFrameworkCore;
using UpSkills.Infrastructure.Entidades;

namespace UpSkills.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Pais> Pais { get; set; }
    public DbSet<Rol> Rol { get; set; }
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Curso> Curso { get; set; }
    public DbSet<Modulo> Modulo { get; set; }
    public DbSet<Material> Material { get; set; }
    public DbSet<Inscripcion> Inscripcion { get; set; }

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

        modelBuilder.Entity<Categoria>(b =>
        {
            b.ToTable(nameof(Categoria), "contenido");
            b.HasKey(c => c.Id);
            b.HasIndex(c => c.Nombre).IsUnique();
        });

        modelBuilder.Entity<Curso>(b =>
        {
            b.ToTable(nameof(Curso), "contenido");
            b.HasKey(c => c.Id);
            b.Property(c => c.Estado).HasConversion<int>();
            b.Property(c => c.CreadoEn)
                .HasColumnType("timestamp without time zone");
            b.HasOne(c => c.Categoria)
                .WithMany(cat => cat.Cursos)
                .HasForeignKey("CategoriaId")
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(c => c.Instructor)
                .WithMany()
                .HasForeignKey("InstructorId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Modulo>(b =>
        {
            b.ToTable(nameof(Modulo), "contenido");
            b.HasKey(m => m.Id);
            b.HasOne(m => m.Curso)
                .WithMany(c => c.Modulos)
                .HasForeignKey("CursoId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Material>(b =>
        {
            b.ToTable(nameof(Material), "contenido");
            b.HasKey(m => m.Id);
            b.Property(m => m.Tipo).HasConversion<int>();
            b.HasOne(m => m.Modulo)
                .WithMany(mod => mod.Materiales)
                .HasForeignKey("ModuloId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Inscripcion>(b =>
        {
            b.ToTable(nameof(Inscripcion), "inscripciones");
            b.HasKey(i => i.Id);
            b.Property(i => i.Estado).HasConversion<int>();
            b.Property(i => i.FechaInscripcion)
                .HasColumnType("timestamp without time zone");
            b.HasOne(i => i.Usuario)
                .WithMany()
                .HasForeignKey("UsuarioId")
                .OnDelete(DeleteBehavior.Restrict);
            b.HasOne(i => i.Curso)
                .WithMany(c => c.Inscripciones)
                .HasForeignKey("CursoId")
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
