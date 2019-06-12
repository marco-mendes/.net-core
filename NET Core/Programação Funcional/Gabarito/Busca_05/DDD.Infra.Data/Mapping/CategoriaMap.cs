using DDD.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.Infra.Data.Mapping
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
	{

    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
      builder.ToTable("Categoria");

      builder.HasKey(c => c.Id);

      builder.HasMany(p => p.CategoriaProduto);

      builder.Property(c => c.Codigo)
        .IsRequired()
        .HasColumnName("Codigo");

    }
	}
}