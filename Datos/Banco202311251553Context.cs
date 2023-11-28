using System;
using System.Collections.Generic;
using InterfazBanco.Datos.ModelosBanco;
using Microsoft.EntityFrameworkCore;

namespace InterfazBanco.Datos;

public partial class Banco202311251553Context : DbContext
{
    public Banco202311251553Context()
    {
    }

    public Banco202311251553Context(DbContextOptions<Banco202311251553Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrador> Administradors { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Cuenta> Cuenta { get; set; }

    public virtual DbSet<TipoCuenta> TipoCuenta { get; set; }

    public virtual DbSet<TipoTransaccion> TipoTransaccions { get; set; }

    public virtual DbSet<TransaccionBancarium> TransaccionBancaria { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Banco");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Administ__3214EC07794C2EA6");

            entity.ToTable("Administrador");

            entity.Property(e => e.Contra)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.TipoAdmin)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cliente__3214EC07A5E0DA53");

            entity.ToTable("Cliente", tb =>
                {
                    tb.HasTrigger("ClienteDespuesInsertar");
                    tb.HasTrigger("ClienteEnLugarEliminar");
                });

            entity.Property(e => e.CorreoE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cuenta__3214EC0796D19165");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Saldo).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Cuenta__IdClient__70DDC3D8");

            entity.HasOne(d => d.TipoCuentaNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.TipoCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cuenta__TipoCuen__6FE99F9F");
        });

        modelBuilder.Entity<TipoCuenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoCuen__3214EC07CF7B94D7");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoTransaccion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoTran__3214EC07C6A411FB");

            entity.ToTable("TipoTransaccion");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TransaccionBancarium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transacc__3214EC07BC7538E1");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Importe).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdCuentaNavigation).WithMany(p => p.TransaccionBancaria)
                .HasForeignKey(d => d.IdCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__IdCue__71D1E811");

            entity.HasOne(d => d.TipoTransaccionNavigation).WithMany(p => p.TransaccionBancaria)
                .HasForeignKey(d => d.TipoTransaccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacci__TipoT__72C60C4A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
