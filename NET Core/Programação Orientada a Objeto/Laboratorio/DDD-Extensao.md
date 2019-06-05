### DDD - Extensão

---

#### Implementando Produto

Nesta parte do laboratório, vamos extender as funcionalidades do nosso serviço anterior adicionando uma nova entidade - Produto.

Com isso, vamos criar o mapeamento, validações, APIs e testes para consolidar os métodos aprendidos até agora. Como base, utilizaremos o código obtido pela finalização do Laboratório inicial sobre DDD -  [DDD-Primeiro](DDD01.md).



#### 1. Adicionando Produto - Camada Domain

Na pasta `DDD.Domain/Entities` você deve adicionar uma nova classe, chamda `Produto` com as propriedades normais que um produto teria, como nome, fabricante, código.

Como cada produto também tem um ID, é possível utilizar como base a `BaseEntity` criada anteriormente.



#### 2. Adicionando Produto - Camada Infra.Data

Aqui vamos mostrar à nossa aplicação como gravar e resgatar um objeto do banco de dados, explicitando o valor de propriedade-coluna. Na pasta `DDD.Infra.Data/Mapping` será necessário criar uma nova classe de mapeamento, podemos chamá-la de `ProdutoMap`, fazendo o bind de cada propriedade do objeto com uma coluna do banco.

> Esta é uma ótima hora para você abrir o [SQLite Browser](https://sqlitebrowser.org/) e modificar o banco de dados SQLite `app.db` localizado na pasta DDD.Application. Você pode adicionar a tabela produtos e também as colunas que você acabou de relacionar com o objeto produto criado. Dessa forma, você completa a parte da infraestrutura de armazenamento do Produto.

Também é necessário especificar no Contexto (SQLiteContext) que a entidade `Produto` é referenciada, adicionando:

```c#
public DbSet<Produto> Produto { get; set; }
```



#### 3. Adicionando Produto - Camada Service

Anteriormente, havíamos criado dois arquivos. Um validador para o objeto User e um Serviço genérico. Como nosso produto é uma extensão da `BaseEntity`, não é necessário criar um novo serviço para nos atender, bastando apenas criar um validador.

Sugestões:

- O preço do produto não pode ser negativo;
- É necessário haver um SKU, assim como um Name e um Código (Interno, por exemplo).



#### 4. Adicionando Produto - Camada Application

Agora com todas as outras camadas implementadas, podemos partir então para a implementação da API em si. Primeiramente temos que criar um Controller para o produto.

Nesta parte, só precisaremos alterar o tipo apontatdo pelo serviço:

```c#
private BaseService<Produto> service = new BaseService<Produto>();
```

E os validadores nos métodos POST e PUT para:

```c#
service.Post<ProdutoValidator>(item);
```



#### 5. Executando e testando

Navegando até a pasta do projeto `DDD.Application` e executando o comando `dotnet run`, podemos testar nossa aplicação manualmente através do Postman.

Agora, podemos realizar uma chamada à URL utilizando um navegador ou o Postman utilizando o método GET. Teremos uma resposta do tipo:

```
[]
```

pois ainda não há nenhum registro no banco de dados.

Podemos também enviar um POST contendo:

```
{
	"Codigo": "153.874.300-02",
	"Fabricante": "OBoticario",
	"Name": "Perfume",
	"Preco": 100,
	"SKU": "324234"
}
```

E nosso GET seria:

```
 [
 		{
        "Codigo": "153.874.300-02",
        "Fabricante": "OBoticario",
        "Name": "Perfume",
        "Preco": 100,
        "SKU": "324234",
        "id": 1
    }
]
```



#### 6. Adicionando testes automáticos

Primeiro, na pasta raiz da solução, vamos criar um projeto de testes do xUnit, com o comando:

```sh
dotnet new xunit -o DDD.Tests

# Adicionando outros projetos
dotnet add DDD.Tests/DDD.Tests.csproj reference DDD.Domain/*.csproj
dotnet add DDD.Tests/DDD.Tests.csproj reference DDD.Service/*.csproj
dotnet add DDD.Tests/DDD.Tests.csproj reference DDD.Infra.Data/*.csproj

cd DDD.Tests

# Adicionando pacotes
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

Como nossos Serviços, Contextos e Repositórios estão atrelados ao banco de dados configurado, vamos criar um novo arquivo definindo novamente, desta vez apontando para o EF InMemory, para que possamos executar os testes sem se preocupar em limpar ou criar o banco de dados.

```c#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Utilizando um servidor SQLite local. Aqui poderíamos configurar qualquer outro banco de dados.
            if (!optionsBuilder.IsConfigured)
                 optionsBuilder.UseInMemoryDatabase(databaseName: "test1");
        }

```

> Um exemplo do arquivo se encontra na pasta assets, chamado TestService.cs

Agora, vamos criar uma classe para testar o Produto. Um exemplo de arquivo de testes é:

```c#
using System;
using Xunit;
using DDD.Infra.Data.Repository;
using DDD.Domain.Interfaces;
using DDD.Domain.Entities;
using DDD.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using DDD.Service.Validators;
using FluentValidation;

namespace DDD.Tests
{
    public class CheckProduto
    {
      	[Theory]
        [InlineData("pdt1", "2", "22", 4, "teste")]
        [InlineData("pdt2", "3", "33", 1, "teste")]
        [InlineData("pdt3", "4", "44", -1, "teste")]
        public void CanCreateAndSave(string name, string codigo, string sku, double preco, string fabricante="")
        {
            TestService<Produto> service = new TestService<Produto>();

            var pdt = new Produto();
            Assert.NotNull(pdt);
            pdt.Name = name;
            pdt.Codigo = codigo;
            pdt.SKU = sku;
            pdt.Preco = preco;
            pdt.Fabricante = fabricante;

            // Se houver erro em validações, deve dar erro ao adicionar no banco.
            if (preco < 0 || name.Length < 1 || sku.Length < 1 || codigo.Length < 1 ) {
                Assert.Throws<FluentValidation.ValidationException>(() => service.Post<ProdutoValidator>(pdt));
                return;
            }
            // Agora criando, resgatando, comparando e deletando.
            service.Post<ProdutoValidator>(pdt);
            Assert.True(pdt.Id > -1);
            var pdt2 = service.Get(pdt.Id);

            Assert.Equal(pdt2.Name, pdt.Name);
            Assert.Equal(pdt2.SKU, pdt.SKU);
            Assert.Equal(pdt2.Fabricante, pdt.Fabricante);

            var id = pdt.Id;
            service.Delete(pdt.Id);

            // Tentando deletar novamente e verificando se Exception é gerada.
            Assert.Throws<System.ArgumentNullException>(() => service.Delete(id));
        }

    }
}
```

Mas dependendo das propriedades de seu Produto, a forma que escolheu validá-los e seus construtores, os casos de teste podem variar. Note que são utilizados `InlineData` para fornecer os mais variados tipos de input ao caso de teste.

