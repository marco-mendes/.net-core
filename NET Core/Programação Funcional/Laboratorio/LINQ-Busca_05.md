## Criando Relações - Parte 4/5

Nesta parte, vamos finalmente gerar migrações para atualizar nosso banco de dados, baseando no EF Core e a auto-geração.



#### Criando Migrações

> Como toda a modelagem de dados das entidades com o banco, incluindo informações sobre a infraestrutura (conexão) estão no projeto Infra.Data, os comandos a seguir devem ser executados referenciando este projeto.

Agora que já modelamos todos os mapeamentos, entidades, serviços e repositórios, vamos seguir para a criação de migrações. Os arquivos de migrações permitem "versionar" o estado do banco de dados à medida que a aplicação é desenvolvida, deixando sempre a opção de rollback quando necessário. Além disso, as migrations permitem manter vários bancos com o mesmo esquema, sempre aplicando somente as alterações necessárias.

Nesta parte, vamos ver alguns comandos do EF para criar e aplicar as migrations em um banco de dados.

> No caso do projeto que estamos codando, o banco de dados utilizado se chama `app.db`, e fica na pasta raiz do projeto em questão.



##### Criando a primeira migration

Para criar a primeira migration, salvando a modelagem atual dos dados da aplicação, basta executar o comando:

```sh
dotnet ef migrations add InitialCreate
```

Desta forma, serão adicionados arquivos à pasta `Migrations` do projeto que contêm informações sobre como o EF deve modificar o banco de dados. 

Ao realizar o comando acima, o EF percorre toda a aplicação, incluindo as entidades e modela um banco baseando-se no que existe e no que foi alterado desde a última migração. 

A cada modificação do código, seja adicionando propriedades ou renomeando tabelas/colunas, é necessário criar uma nova migration, versionando assim as modificações feitas.



##### Aplicando a migration

Ao digitar o comando abaixo, o EF irá procurar o banco especificado no contexto do projeto e irá realizar as migrações necessárias. Como estamos no projeto Infra.Data, será criado um arquivo `app.db` na raiz do projeto, contendo já as tabelas e colunas necessárias para a aplicação ser executada.

```bash
dotnet ef database update
```

Agora, vamos copiar o arquivo `app.db` para a pasta raiz do projeto `Application`, para que ele consiga acessar o banco.

> Agora é uma boa hora para abrir o banco com um editor SQLite e analisar o que o EF criou de forma automática, incluindo chaves estrangeiras e tabelas de controle.

