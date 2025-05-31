using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Empresa2.Models;

public partial class EmpresaContext : DbContext
{
    public EmpresaContext()
    {
    }

    public EmpresaContext(DbContextOptions<EmpresaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleados> Empleados { get; set; }

    public virtual DbSet<Secciones> Secciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=Empresa;password=root;port=3307", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.3.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Empleados>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.HasIndex(e => e.IdSeccion, "fkSeccionEmpleado");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Eliminar).HasDefaultValueSql("'0'");
            entity.Property(e => e.IdSeccion).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Sueldo).HasPrecision(10);

            entity.HasOne(d => d.IdSeccionNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdSeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkSeccionEmpleado");
        });

        modelBuilder.Entity<Secciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("secciones");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Eliminar).HasDefaultValueSql("'0'");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.NumeroEmpleados).HasColumnType("int(11)");
            entity.Property(e => e.SueldoMaximo).HasPrecision(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
