#### Exercícios Práticos sobre Orientação por Objeto - Parte 1

Aqui veremos exercícios práticos relacionados aos conceitos de Classe, Construtores, Composição, Herança, Proteção e Interface.

> Por padrão, pode ser criado uma solução ou um projeto, utilizando os comandos:
>
> dotnet new sln -o <nome da solucao>
>
> cd <nome da solucao>
>
> dotnet new console -o <nome do projeto>
>
> dotnet sln add <nome do projeto>
>
>
> Por mais referências, veja os laboratórios antigos, como:
>  [Console.md](../../NET Core 2.2/Laboratório/Console.md) e  [xUnit.md](../../NET Core 2.2/Laboratório/xUnit.md) 



1. Analisando o ambiente de sua empresa, Cláudio percebeu que estava na hora de automatizar alguns aspectos, a fim de reduzir a quantidade de papel guardada em seus escaninhos. Ele precisa de uma organização de dados que permita configurar uma filial, os tipos de produtos com que aquela filial trabalha e seus funcionários. Para facilitar, anotamos alguns requisitos e demos nomes a algumas classes que você pode precisar em sua implementação.

   - Classe Pessoa (Classe)
     - Regras de negócio:
       - Deve definir se é física ou jurídica;
       - Deve receber dados de uma pessoa, como endereço, nome e identificador.

   - Classe Funcionário (Classe)
     - Pode herdar dados e métodos de Pessoa - tipo Física
     - Regras de negócio:
       - Deve ser possível especificar quantos produtos o funcionário vendeu até o momento;
       - Só a própria filial pode alterar seus funcionários  (Proteção/Interface).
   - Classe Produto (Classe)
     - Regras de negócio:
       - Como o programa não faz controle de estoque, basta guardar informações básicas sobre o produto, como tipo, nome e fabricante;
       - Só a própria filial pode alterar os produtos com que trabalha  (Proteção/Interface);
   - Classe Filial (Classe, Herança)
     - Pode herdar dados e métodos de Pessoa - tipo Jurídica;
     - Regras de negócio:
       - Deve ter propriedades que a definam, seja endereço ou CNPJ;
       - Deve conter uma coleção de funcionários (Composição);
       - Deve conter uma coleção de tipos de produtos  (Composição);
       - Quando uma filial é criada, deve vir com alguns tipos de produto padrões e nenhum funcionário (Construtores).



Uma parte do código já foi desenvolvida (incluindo [Interfaces](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/interfaces/) e [Classes](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/classes-and-structs/classes)), mas sinta-se livre para alterá-la como desejar:

```C#
enum TipoPessoa {Fisica, Juridica};

// Definindo interface
interface IPessoa {
    string Nome {get;set;}
    string Endereco {get;set;}
    string Identificador {get;set;}
    TipoPessoa Tipo {get; set;}
}

// Implementando Interface
class Pessoa : IPessoa {
    private string identificador, nome;
    private TipoPessoa tipo;
  	public string Endereco {get; set;}

    public string Nome {
        get {
            return this.nome;
        } 
        set {
            this.nome = value;
        }
    }
    public TipoPessoa Tipo { 
        get {
            return this.tipo;
        }
        set {
            // Verificando se tipo passado é definido
            if (Enum.IsDefined(typeof(TipoPessoa), value)) {
                this.tipo = value;
            }
        } 
    }
    public string Identificador { 
        get {
            return this.identificador;
        } 
        set {
            this.identificador = value;
        }
    }
}
/* Exemplo de criação de objeto Pessoa Jurídica:
Pessoa pj0 = new Pessoa () { Tipo = TipoPessoa.Juridica, 
														 Nome = "Filial 1",
                             Identificador = "59.191.411/0001-54",
                             Endereco = "Rua Imaginaria numero 0"};
*/
```





2. Analisando a eficiência e rapidez que seu programa deu à organização dos negócios, Cláudio pediu que você implementasse também um controle de clientes. Só tem um problema: nem todo mundo está disposto a dar um número identificador (CPF ou RG). Então seria interessante se os valores de "id" de uma Pessoa pudessem ser nulos por padrão, utilizando os [Tipos de Dados Anuláveis](https://docs.microsoft.com/pt-br/dotnet/csharp/nullable-references) do C#. 

   E outra, cada cliente pode ter um ou mais endereços relacionados, então é uma boa hora para utilizar um método de 'armazenamento' como [Tuplas](https://docs.microsoft.com/pt-br/dotnet/csharp/tuples).

