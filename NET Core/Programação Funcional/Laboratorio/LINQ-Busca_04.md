### Criando Relações - Parte 3/5

Nesta parte vamos criar o mapeamento das nossas entidades, utilizando do EF Core para automatizar a criação de campos. O escopo aqui é diretamente ligado à infraestrutura, então é recomendado que os passos aqui sejam feitos no projeto Infra.Data.



#### Criando/Atualizando repositório e serviços

Nesta parte, vamos fazer a ligação entre as entidades, o banco e suas funcionalidades. Em cada passo (seção), deve ser criado um repositório e serviço que contenha funções de manipulação do objeto.

Nos próximos passos, vamos criar/atualizar os repositórios e serviços de Produto, Fabricante e Categoria. Desta forma, teremos os métodos de armazenamento e criação de relações definidos e prontos para serem implementados pelos controladores da API.

- Objetivos:
  - Deve ser possível realizar o CRUD completo de Categorias, Fabricantes e Produtos;
  - Ao ler uma Categoria ou um Fabricante, deve ser retornado também os produtos relacionados;
  - Só deve ser possível alterar as Categorias de um Produto através do Serviço de Produtos;
  - Deve haver um serviço em Produto que aceita query por LINQ (Func<T, bool>)



O CRUD completo de Categoria e Fabricante é feito naturalmente pelo `BaseService`, responsável por tratar as entidades que herdam a `BaseEntity`, já que nesta etapa, não é necessário estabelecer funcionalidades extras.

O ponto do exercício então é criar um repositório e serviço de produto, que dado um Produto, um Array de Categorias e um Fabricante, deve relacionar todos.



---

**Informações extras:**

#### Sobre EF Core

O EF Core possui funcionalidades que auxiliam os desenvolvedores a criar aplicações utilizando a premissa `Code First`. Dessa forma, o time consegue criar as entidades, as relações e as dependências somente no código, criando uma camada de abstração do modelo de banco que utilizariam.

O EF Core pode servir como um ORM (Mapeador de Objeto Relacional), permitindo que os desenvolvedores de .NET trabalhem com um banco de dados usando objetos do .NET e eliminando a necessidade de grande parte do código de acesso aos dados que eles geralmente precisam escrever.



#### Consultar

Instâncias de suas classes de entidade são recuperadas do banco de dados usando a LINQ (Consulta Integrada à Linguagem). Consulte [Consultar dados](https://docs.microsoft.com/pt-br/ef/core/querying/index) para saber mais.



#### Salvar Dados

Dados são criados, excluídos e modificados no banco de dados usando as instâncias de suas classes de entidade. Consulte [Salvar dados](https://docs.microsoft.com/pt-br/ef/core/saving/index) para saber mais.

Dados relacionados: https://docs.microsoft.com/pt-br/ef/core/saving/related-data