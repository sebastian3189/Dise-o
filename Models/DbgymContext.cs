using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GYM_ITM.Models;

public partial class DbgymContext : DbContext
{
    public DbgymContext()
    {
    }

    public DbgymContext(DbContextOptions<DbgymContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actividades> Actividades { get; set; }

    public virtual DbSet<Espacio> Espacios { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
          if (!optionsBuilder.IsConfigured)
        {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
       // => optionsBuilder.UseSqlServer("Server=DESKTOP-A3FHGUN\\SQLEXPRESS; Database=DBGYM; Trusted_Connection=True;TrustServerCertificate=true;");
        }
    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Server=DESKTOP-A3FHGUN\\SQLEXPRESS; Database=DBGYM; Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actividades>(entity =>
        {
            entity.HasKey(e => e.IdActividad).HasName("PK__Activida__5EAF86A40B737269");

            entity.Property(e => e.DescripcionActividad).HasMaxLength(255);
            entity.Property(e => e.NombreActividad).HasMaxLength(255);
        });

        modelBuilder.Entity<Espacio>(entity =>
        {
            entity.HasKey(e => e.IdEspacio).HasName("PK__Espacios__CA4C0889747AF17A");

            entity.Property(e => e.NombreEspacio).HasMaxLength(255);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__1539229B93EFD8E6");

            entity.HasIndex(e => e.IdEspacio).IsUnique(false);

            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");

            entity.HasOne(d => d.IdActividadNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdActividad)
                .HasConstraintName("FK__Horarios__IdActi__4E88ABD4");

            entity.HasOne(d => d.IdEspacioNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.IdEspacio)
                .HasConstraintName("FK__Horarios__IdEspa__4F7CD00D");

        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF976F241567");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasMany(d => d.IdHorarios).WithMany(p => p.IdUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioHorario",
                    r => r.HasOne<Horario>().WithMany()
                        .HasForeignKey("IdHorario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioHo__IdHor__5535A963"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UsuarioHo__IdUsu__5441852A"),
                    j =>
                    {
                        j.HasKey("IdUsuario", "IdHorario").HasName("PK__UsuarioH__FA362DBEFC8EF197");
                        j.ToTable("UsuarioHorario");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
