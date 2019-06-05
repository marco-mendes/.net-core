using DDD.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.Data.Mapping
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
	{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
      builder.ToTable("Produto");

      builder.HasKey(c => c.Id);

      builder.Property(c => c.Fabricante)
        .IsRequired()
        .HasColumnName("Fabricante");

      builder.Property(c => c.Codigo)
        .IsRequired()
        .HasColumnName("Codigo");

      builder.Property(c => c.Preco)
        .IsRequired()
        .HasColumnName("Preco");

      builder.Property(c => c.SKU)
        .IsRequired()
        .HasColumnName("SKU");

      builder.Property(c => c.Name)
        .IsRequired()
        .HasColumnName("Name");
    }
	}
}