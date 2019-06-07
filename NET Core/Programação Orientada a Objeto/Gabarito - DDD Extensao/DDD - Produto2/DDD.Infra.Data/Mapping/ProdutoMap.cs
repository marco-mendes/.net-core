using DDD.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.Infra.Data.Mapping
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
	{

    [ForeignKey("FabricanteForeignKey")]
    public Fabricante Fabricante;
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
      builder.ToTable("Produto");

      builder.HasKey(c => c.Id);

      builder.HasOne(p => p.Fabricante)
            .WithMany(b => b.Produtos)
            .HasForeignKey("FabricanteId");

      builder.Property(c => c.Codigo)
        .IsRequired()
        .HasColumnName("Codigo");

      builder.Property(c => c.Preco)
        .IsRequired()
        .HasColumnName("Preco");

      builder.Property(c => c.Sku)
        .IsRequired()
        .HasColumnName("Sku");

      builder.Property(c => c.Name)
        .IsRequired()
        .HasColumnName("Name");
    }
	}
}