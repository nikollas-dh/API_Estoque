using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API_Estoque.Models;

public partial class DbEstoqueContext : DbContext
{
    public DbEstoqueContext()
    {
    }

    public DbEstoqueContext(DbContextOptions<DbEstoqueContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Materiai> Materiais { get; set; }

    public virtual DbSet<Movimentacao> Movimentacaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=dbEstoque;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Materiai>(entity =>
        {
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Quantidade).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UnidadeMedida)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Unidade_Medida");
            entity.Property(e => e.ValorUnitario)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Valor_Unitario");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Materiais)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_Materiais_Categoria");
        });

        modelBuilder.Entity<Movimentacao>(entity =>
        {
            entity.ToTable("Movimentacao");

            entity.Property(e => e.DataLancamento).HasColumnType("datetime");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ValorUnitario)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("Valor_Unitario");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.Movimentacaos)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Movimentacao_Materiais");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
