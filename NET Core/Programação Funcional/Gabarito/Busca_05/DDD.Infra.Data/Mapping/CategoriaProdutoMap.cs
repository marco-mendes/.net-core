using DDD.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDD.Infra.Data.Mapping
{
    public class CategoriaProdutoMap : IEntityTypeConfiguration<CategoriaProduto>
	{
    public void Configure(EntityTypeBuilder<CategoriaProduto> builder)
    {
      builder.ToTable("CategoriaProduto");
      
      builder.HasKey(cp => new { cp.CategoriaId, cp.ProdutoId });

      builder.HasOne<Categoria>(cp => cp.Categoria)
        .WithMany(p => p.CategoriaProduto)
        .HasForeignKey(cp => cp.CategoriaId);

      builder.HasOne<Produto>(cp => cp.Produto)
        .WithMany(c => c.CategoriaProduto)
        .HasForeignKey(cp => cp.ProdutoId);


    }
	}
}