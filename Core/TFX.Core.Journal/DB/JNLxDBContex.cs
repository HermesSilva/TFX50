using System;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;

using Sittax.Domain.Core;

using TFX.Core;
using TFX.Core.Exceptions;

namespace TFX.Journal.DB;

public class JNLxDBContex : XDBContext
{
    public JNLxDBContex()
            : base(new DbContextOptions<JNLxDBContex>())
    {
    }

    public virtual DbSet<Campos> Campos
    {
        get; set;
    }

    public virtual DbSet<Selects> Selects
    {
        get; set;
    }

    public virtual DbSet<Tabelas> Tabelas
    {
        get; set;
    }

    protected override String GetConnectionString()
    {
        string stringConn = "";
        if (!string.IsNullOrEmpty(stringConn))
        {
            DbConnectionStringBuilder cb = new DbConnectionStringBuilder();
            cb.ConnectionString = stringConn;
            cb["Initial Catalog"] = "XZ";
            return stringConn;
        }
        throw new XError("Value for evironment variable 'SQL_SERVER_CONEXAO_JNL' is not found");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campos>(entity =>
        {
            entity.HasKey(e => e.idCampos).HasName("PK_Journal_Campos");

            entity.ToTable("Campos");

            entity.HasIndex(e => new { e.Nome, e.idTabelas }, "IDX_Journal_Campos").IsUnique();

            entity.Property(e => e.Nome)
                            .IsRequired()
                            .HasMaxLength(128)
                            .IsUnicode(false);
            entity.Property(e => e.Type)
                            .IsRequired()
                            .HasMaxLength(30)
                            .IsUnicode(false);
        });

        modelBuilder.Entity<Selects>(entity =>
        {
            entity.HasKey(e => e.idSelects).HasName("PK_Journal_Selects");

            entity.ToTable("Selects");

            entity.Property(e => e.Nome)
                            .IsRequired()
                            .HasMaxLength(128)
                            .IsUnicode(false);
            entity.Property(e => e.Query)
                            .IsRequired()
                            .HasColumnType("text");
            entity.Property(e => e.QueryDado)
                            .IsRequired()
                            .HasColumnType("text");
        });

        modelBuilder.Entity<Tabelas>(entity =>
        {
            entity.HasKey(e => e.idTabelas).HasName("PK_Journal_Tabelas");

            entity.ToTable("Tabelas");

            entity.HasIndex(e => new { e.Nome, e.Esquema }, "IDX_Journal_Tabelas").IsUnique();

            entity.Property(e => e.CampoPK)
                            .IsRequired()
                            .HasMaxLength(128)
                            .IsUnicode(false);
            entity.Property(e => e.Esquema)
                            .IsRequired()
                            .HasMaxLength(128)
                            .IsUnicode(false);
            entity.Property(e => e.Nome)
                            .IsRequired()
                            .HasMaxLength(128)
                            .IsUnicode(false);
        });
    }
}
