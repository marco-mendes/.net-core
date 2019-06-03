#### Exercícios Práticos sobre Orientação a Objeto

Aqui veremos exercícios práticos relacionados aos conceitos de Classe, Construtores, Composição, Herança, Proteção e Interface.

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
       - Quando uma filial é criada, deve vir com alguns tipos de produto padrões e nenhum funcionário (Construtores);
       - Deve conter uma coleção de tipos de produtos  (Composição).



Uma parte do código já foi desenvolvida, mas sinta-se livre para alterá-la como desejar:

```C#
enum TipoPessoa {Fisica, Juridica};
class Pessoa {
    // Definindo variáveis - privadas
    private string identificador, nome;
    private TipoPessoa tipo;
  	public string Endereco {get; set;}

    // Definindo interfaces - públicas
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
            // Verificando se tipo de pessoa passado é definido
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



3. Um belo dia, Cláudio foi disfarçado em uma de suas filiais e foi muito bem atendido por João da Silva, um funcionário. Ele gostou tanto do atendimento que resolveu enviar um brinde para casa de João. Só tem um problema, ele não sabe o endereço dele. Como Cláudio pretende visitar com frequência suas lojas e verificar o atendimento, gostaria que seu programa implementasse uma função de pesquisa de funcionário por filial, seja por Nome ou Identificador. 

