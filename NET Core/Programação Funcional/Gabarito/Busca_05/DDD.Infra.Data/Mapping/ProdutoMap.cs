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

    public void Configure(EntityTypeBuilder<Produto> builder)
    {
      builder.ToTable("Produto");

      builder.HasKey(c => c.Id);

      builder.HasOne(p => p.Fabricante);

      builder.HasMany(p => p.CategoriaProduto);

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