### Criando Relações - Parte 1/5

Para aprofundar um pouco mais no quesito de buscas de modelos em banco de dados utilizando LINQ, vamos primeiro estabelecer entidades e relações na nossa aplicação. O escopo aqui é diretamente ligado ao Domínio, então é recomendado que os passos aqui sejam feitos no projeto Domain.



#### Criando/Atualizando Entidades

Nesta parte, criaremos entidades que possuem propriedades que se relacionam diretamente à entidades externas. Em cada passo (seção), deve ser criado um modelo que contenha os campos que achar necessário, além de listas e propriedades que estabeleçam uma relação entre eles.

Nos próximos passos, vamos criar/atualizar as entidades de Produto, Fabricante e Categoria. Desta forma, teremos relações do tipo "Um para muitos" e "Muitos para Muitos", sendo também verdadeira a inversa.



##### Fabricante

Por convenção, cada entidade pode herdar da entidade base `BaseEntity` previamente criada. Como exemplo, a entidade `Fabricante` pode herdar suas as propriedades da classe base se for declarada da forma:

```c#
public class Fabricante : BaseEntity
```

Naturalmente, é esperado que uma entidade `Fabricante` possua uma lista de produtos relacionados, sendo declarada por exemplo por:

```c#
public virtual ICollection<Produto> Produtos {get; set;}
```

Um exemplo então de declaração de classe `Fabricante` é:

```c#
public class Fabricante : BaseEntity
    {
        public string Name { get; set; }
        public string Codigo { get; set; }
        public virtual ICollection<Produto> Produtos {get; set;}

    }
```

> Sinta-se livre para adicionar ou remover propriedades da classse.



##### Produto

Por convenção, se cada produto possui um Id único, podemos reutilizar novamente a classe previamente definida com o `BaseEntity`, levando a declaração de produto ao formato:

```c#
public class Produto : BaseEntity
```

Naturalmente, é esperado que uma entidade `Produto` possua um tipo `Fabricante` relacionado. Além disso, um produto pode pertencer/se relacionar a várias categorias. Dessa forma, podemos declarar então as propriedades como:

```c#
public virtual Fabricante Fabricante { get; set; }
public virtual ICollection<CategoriaProduto> CategoriaProduto {get; set;}
```

> A utilização da classe de ligação CategoriaProduto será explorada mais à frente.



##### Categoria

Por convenção, se cada categoria possui um Id único, novamente podemos herdar da classe pai `BaseEntity`, levando a declaração ao:

```c#
public class Categoria : BaseEntity
```

Uma categoria também pode ter vários tipo `Produto` relacionados, então vamos adicionar uma propriedade para isso:

```c#
public virtual ICollection<CategoriaProduto> CategoriaProduto {get; set;}
```



##### CategoriaProduto

A ligação entre modelos/entidades do Entity Framework 6 é diferente do EF Core. Para que possamos obedecer a premissa `Code First` e manter a compatibilidade do projeto com o padrão .NET Core, será necessário criar uma entidade (classe) de relacionamento entre Categoria e Produto.

```c#
public class CategoriaProduto
    {
        public int CategoriaId { get; set; }
        public Categoria Categoria {get; set;}
        public int ProdutoId { get; set; }
        public Produto Produto {get; set;}
    }
```

