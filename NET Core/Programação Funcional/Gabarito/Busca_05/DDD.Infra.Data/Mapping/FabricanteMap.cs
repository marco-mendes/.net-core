using DDD.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Infra.Data.Mapping
{
    public class FabricanteMap : IEntityTypeConfiguration<Fabricante>
	{
    public void Configure(EntityTypeBuilder<Fabricante> builder)
    {
      builder.ToTable("Fabricante");

      builder.HasKey(c => c.Id);

      builder.Property(c => c.Name)
        .IsRequired()
        .HasColumnName("Name");

      builder.HasMany(c => c.Produtos).WithOne(e => e.Fabricante)
            .HasForeignKey("FabricanteId");
      
      builder.Property(c => c.Codigo)
        .IsRequired()
        .HasColumnName("Codigo");

    }
	}
}