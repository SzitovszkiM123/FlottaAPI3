using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace FlottaAPI.Models;

public partial class FlottaContext : DbContext
{
    public FlottaContext()
    {
    }

    public FlottaContext(DbContextOptions<FlottaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gepjarmu> Gepjarmus { get; set; }

    public virtual DbSet<Kikuldete> Kikuldetes { get; set; }

    public virtual DbSet<Kikuldottjarmu> Kikuldottjarmus { get; set; }

    public virtual DbSet<Sofor> Sofors { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_hungarian_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Gepjarmu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("gepjarmu")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Marka).HasMaxLength(16);
            entity.Property(e => e.Rendszam).HasMaxLength(8);
            entity.Property(e => e.Tipus).HasMaxLength(16);
            entity.Property(e => e.Ulesek).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Kikuldete>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("kikuldetes")
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Befejezes).HasColumnType("datetime");
            entity.Property(e => e.Celja).HasColumnType("text");
            entity.Property(e => e.Cim).HasMaxLength(128);
            entity.Property(e => e.Kezdes).HasColumnType("datetime");
        });

        modelBuilder.Entity<Kikuldottjarmu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("kikuldottjarmu")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.GepjarmuId, "GepjarmuId");

            entity.HasIndex(e => e.KikuldetesId, "KikuldetesId");

            entity.HasIndex(e => e.Sofor, "Sofor");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.GepjarmuId).HasColumnType("int(11)");
            entity.Property(e => e.KikuldetesId).HasColumnType("int(11)");
            entity.Property(e => e.Sofor).HasColumnType("int(11)");

            entity.HasOne(d => d.Gepjarmu).WithMany(p => p.Kikuldottjarmus)
                .HasForeignKey(d => d.GepjarmuId)
                .HasConstraintName("kikuldottjarmu_ibfk_3");

            entity.HasOne(d => d.Kikuldetes).WithMany(p => p.Kikuldottjarmus)
                .HasForeignKey(d => d.KikuldetesId)
                .HasConstraintName("kikuldottjarmu_ibfk_1");

            entity.HasOne(d => d.SoforNavigation).WithMany(p => p.Kikuldottjarmus)
                .HasForeignKey(d => d.Sofor)
                .HasConstraintName("kikuldottjarmu_ibfk_2");
        });

        modelBuilder.Entity<Sofor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sofor")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(64);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.Phone).HasMaxLength(32);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("user")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.HasIndex(e => e.LoginName, "LoginName").IsUnique();

            entity.HasIndex(e => e.Permission, "Permission");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Address).HasMaxLength(256);
            entity.Property(e => e.Hash).HasMaxLength(255);
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.LoginName).HasMaxLength(64);
            entity.Property(e => e.Name).HasMaxLength(64);
            entity.Property(e => e.Permission).HasColumnType("int(11)");
            entity.Property(e => e.Phone).HasMaxLength(32);
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','active','suspended','banned')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
