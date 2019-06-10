### Criando Relações - Parte 2/5

Nesta parte vamos criar o mapeamento das nossas entidades, utilizando do EF Core para automatizar a criação de campos. O escopo aqui é diretamente ligado à infraestrutura, então é recomendado que os passos aqui sejam feitos no projeto Infra.Data.



#### Criando/Atualizando Mapeamento

Nesta parte, vamos fazer a ligação entre as entidades e suas propriedades com o banco de dados. Em cada passo (seção), deve ser criado um mapeamento que contenha as listas e propriedades que estabeleçam uma relação entre eles.

Nos próximos passos, vamos criar/atualizar os mapeamentos de Produto, Fabricante e Categoria. Desta forma, teremos relações do tipo "Um para muitos" e "Muitos para Muitos", sendo também verdadeira a inversa.



##### FabricanteMap

No exemplo abaixo, estamos mapeando propriedades diretamente à colunas do banco. Além disso, definimos a propriedade `c.Produtos` que está diretamente ligada à entidade `Produtos` do `Fabricante`.

```c#
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
```

> Sinta-se livre para adicionar ou remover propriedades do mapeamento.
>
> Note que o `HasMany` diz para a biblioteca de mapeamento que o objeto retratado possui em cada produto uma referência única para um `Fabricante`. Dessa forma, o EF Core consegue trazer as relações e devolvê-las para a aplicação.



##### ProdutoMap

O mapeamento da entidade `Produto` é um pouco diferente, por conter os dois tipos de relação:

```c#
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
```

> Note a utilização de `HasOne` para definir o `Fabricante`. Isso indica ao EF Core que a chave estrangeira do Fabricante deve ser guardada com cada Produto, e irá adicionar um mapemanto de `FKFabricante` à tabela de Produto.



##### CategoriaMap

O mapeamento de categoria é bem parecido com o de `Produto`, removendo algumas propriedades.

```c#
public void Configure(EntityTypeBuilder<Categoria> builder)
{
  builder.ToTable("Categoria");

  builder.HasKey(c => c.Id);

  builder.HasMany(p => p.CategoriaProduto);

  builder.Property(c => c.Codigo)
    .IsRequired()
    .HasColumnName("Codigo");

}
```

> Note que tanto a Categora quando Produto fazem relação à outra Entidade, CategoriaProduto. É nela que devemos declarar então a relação entre Categoria <-> Produto.



##### CategoriaProdutoMap

Neste mapeamento, só nos resta então mapear diretamente um `Produto` e uma `Categoria`, da forma:

```c#
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
```

