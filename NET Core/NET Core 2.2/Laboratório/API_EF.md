## Laboratório de API

Este tutorial irá demonstrar como criar API mínima em ASP.NET Core com o ASP.NET Web API,  com todos os componentes necessários para ser categorizada como CRUD:

| API                   | Descrição                                 | Corpo da solicitação      | Corpo da resposta                    |
| :-------------------- | :---------------------------------------- | :------------------------ | :----------------------------------- |
| GET /api/todo         | Obter todos os itens de tarefas pendentes | Nenhum                    | Matriz de itens de tarefas pendentes |
| GET /api/todo/{id}    | Obter um item por ID                      | Nenhum                    | Item de tarefas pendentes            |
| POST /api/todo        | Adicionar um novo item                    | Item de tarefas pendentes | Item de tarefas pendentes            |
| PUT /api/todo/{id}    | Atualizar um item   existente             | Item de tarefas pendentes | Nenhum                               |
| DELETE /api/todo/{id} | Excluir um item                           | Nenhum                    | Nenhum                               |

O diagrama a seguir mostra o design do aplicativo.

![O cliente é representado por uma caixa à esquerda e envia uma solicitação e recebe uma resposta do aplicativo, uma caixa desenhada à direita.](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api/_static/architecture.png?view=aspnetcore-2.2)

Este laboratório será um pouco diferente dos anteriores. A cada novo pedaço de código introduzido será necessário parar a execução e rodar novamente com o comando `dotnet run`. 

A cada adição de método HTTP da API serão feitos testes para verificar seu funcionamento utilizando o Postman.

## Pré-requisitos

- [Visual Studio Code](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api?view=aspnetcore-2.2&tabs=visual-studio#tabpanel_CeZOj-G++Q_visual-studio-code)

- [SDK 2.2 ou posterior do .NET Core](https://www.microsoft.com/net/download/all)

- [Postman](https://www.getpostman.com/apps)

  



## Criar um projeto Web

Como todos os laboratórios anteriores, criaremos o projeto através da linha de comando. Para isso, basta executar:

```
dotnet new webapi -o lab-api
```

O comando acima vai por padrão criar uma nova pasta chamada `lab-api` e um projeto dentro, restaurando automaticamente as dependências.

### Testar a API

Para export a API, precisamos navegar até a pasta do projeto e executar o comando `dotnet run`:

```
cd lab-api
dotnet run
```

> Por padrão, a porta HTTP é 5000 e HTTPS é 5001.

O modelo de projeto webapi criado anteriormente começa com uma API `values`. Ao chamar o método `Get` em um navegado apenas acessando o endereço http://localhost:5000/api/values ou https://localhost:5001/api/values

O seguinte JSON é retornado:

JSONCopiar

```json
["value1","value2"]
```



## Adicionar uma classe de modelo

Um *modelo* é um conjunto de classes que representam os dados gerenciados pelo aplicativo. O modelo para esse aplicativo é uma única classe `TodoItem`.

Para adicionar o modelo, vamos primeiro criar uma pasta chamada Models com o comando:

````sh
mkdir Models
````

Agora, para criar o modelo, vamos criar um novo arquivo no VSCode:

- Clique com o botão direito do mouse na pasta *Models* e selecione **Novo Arquivo**. Dê ao arquivo o nome *TodoItem*.cs
- Substitua o código do modelo pelo seguinte código:

```csharp
namespace lab_api.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
```

A propriedade `Id` funciona como a chave exclusiva em um banco de dados relacional.

As classes de modelo podem ser colocadas em qualquer lugar no projeto, mas a pasta *Models* é usada por convenção e organização.

## Adicionar um contexto de banco de dados

O *contexto de banco de dados* é a classe principal que coordena a funcionalidade do Entity Framework para um modelo de dados.Essa classe é criada derivando-a da classe `Microsoft.EntityFrameworkCore.DbContext`.

- Clique com o botão direito do mouse na pasta *Models* e selecione **Novo Arquivo**. Nomeie-o como *TodoContext.cs* e clique em **Adicionar**.

- Substitua o código do modelo pelo seguinte código:

  ```csharp
  using Microsoft.EntityFrameworkCore;
  
  namespace lab_api.Models
  {
      public class TodoContext : DbContext
      {
          public TodoContext(DbContextOptions<TodoContext> options)
              : base(options)
          {
          }
  
          public DbSet<TodoItem> TodoItems { get; set; }
      }
  }
  ```

## Registrar o contexto de banco de dados

No ASP.NET Core, serviços como o contexto de BD precisam ser registrados no contêiner de [DI (injeção de dependência)](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2). O contêiner fornece o serviço aos controladores.

Atualize *Startup.cs* com o seguinte código realçado:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using lab_api.Models;

namespace lab_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the 
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP 
        //request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
```

O código anterior:

- Remove as declarações `using` não utilizadas.
- Adiciona o contexto de banco de dados ao contêiner de DI.
- Especifica que o contexto de banco de dados usará um banco de dados em memória.

## Adicionar um controlador

- Clique com o botão direito do mouse na pasta *Controllers*.
- Selecione **Adicionar** > **Novo Item**.
- Clique com o botão direito do mouse na pasta *Controllers* e selecione **Novo Arquivo**. Nomeie-o como *TodoController.cs* e clique em **Adicionar**.

- Substitua o código do modelo pelo seguinte código:

  ```csharp
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.EntityFrameworkCore;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using lab_api.Models;
  
  namespace lab_api.Controllers
  {
      [Route("api/[controller]")]
      [ApiController]
      public class TodoController : ControllerBase
      {
          private readonly TodoContext _context;
  
          public TodoController(TodoContext context)
          {
              _context = context;
  
              if (_context.TodoItems.Count() == 0)
              {
                  // Create a new TodoItem if collection is empty,
                  // which means you can't delete all TodoItems.
                  _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                  _context.SaveChanges();
              }
          }
      }
  }
  ```

O código anterior:

- Define uma classe de controlador de API sem métodos.

- Decora a classe com o atributo [[ApiController\]](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.mvc.apicontrollerattribute). Esse atributo indica se o controlador responde às solicitações da API Web. Para saber mais sobre comportamentos específicos habilitados pelo atributo, consulte [Criar APIs Web com o ASP.NET Core](https://docs.microsoft.com/pt-br/aspnet/core/web-api/index?view=aspnetcore-2.2).

- Usa a DI para injetar o contexto de banco de dados (`TodoContext`) no controlador. O contexto de banco de dados é usado em cada um dos métodos [CRUD](https://wikipedia.org/wiki/Create,_read,_update_and_delete) no controlador.

- Adiciona um item chamado `Item1` ao banco de dados se e somente se o banco de dados está vazio. Esse código está no construtor, de modo que ele seja executado sempre que há uma nova solicitação HTTP. Se você excluir todos os itens, o construtor criará `Item1` novamente na próxima vez que um método de API for chamado. Então, se houver somente um item no banco de dados, ao excluí-lo, um item padrão `Item1`será automaticamente adicionado.

  

## Adicionar métodos Get

Para fornecer uma API que recupera itens salvos, adicione os seguintes métodos à classe `TodoController`:

```csharp
// GET: api/Todo
[HttpGet]
public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
{
    return await _context.TodoItems.ToListAsync();
}

// GET: api/Todo/5
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
{
    var todoItem = await _context.TodoItems.FindAsync(id);

    if (todoItem == null)
    {
        return NotFound();
    }

    return todoItem;
}
```

> Todos os métodos de HTTP Request da Classe TodoController devem estar dentro da classe!

Esses métodos implementam dois pontos de extremidade GET:

- `GET /api/todo`
- `GET /api/todo/{id}`

Teste o aplicativo chamando os dois pontos de extremidade em um navegador. Por exemplo:

- `https://localhost:<port>/api/todo`
- `https://localhost:<port>/api/todo/1`

A seguinte resposta HTTP é produzida pela chamada a `GetTodoItems`:

```json
[
  {
    "id": 1,
    "name": "Item1",
    "isComplete": false
  }
]
```

## Roteamento e caminhos de URL

O atributo [`[HttpGet]`](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.mvc.httpgetattribute) indica um método que responde a uma solicitação HTTP GET. O caminho da URL de cada método é autmomaticamente construído da seguinte maneira:

- No início do arquivo temos o  atributo `Route` do controlador:

  ```csharp
  namespace lab_api.Controllers
  {
      [Route("api/[controller]")]
      [ApiController]
      public class TodoController : ControllerBase
      {
          private readonly TodoContext _context;
  ```

- A palavra `[controller]` é automaticamente substituída pelo nome do controlador durante a execução, que é o nome de classe do controlador menos o sufixo "Controller" por convenção. Para esta amostra, o nome da classe do controlador é **Todo**Controller e, portanto, o nome do controlador é "todo". O [roteamento](https://docs.microsoft.com/pt-br/aspnet/core/mvc/controllers/routing?view=aspnetcore-2.2) do ASP.NET Core não diferencia maiúsculas de minúsculas.

- Se o atributo `[HttpGet]` tiver um outro sufixo (por exemplo, `[HttpGet("all")]`), o sufixo é automaticamente acrescento à URL no mapeamento. Portanto, para acessar o endpoint, é utilizada a URL `http://localhost:<port>/api/todo/all` .

No método `GetTodoItem` a seguir, `"{id}"` é uma variável de espaço reservado para o identificador exclusivo do item requisitado. Quando `GetTodoItem` é invocado, o valor de `"{id}"` na URL é fornecido para o método no parâmetro `id`.

```csharp
// GET: api/Todo/5
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
{
    var todoItem = await _context.TodoItems.FindAsync(id);

    if (todoItem == null)
    {
        return NotFound();
    }

    return todoItem;
}
```

## Valores de retorno

O tipo de retorno dos métodos `GetTodoItems` e `GetTodoItem` é o [tipo ActionResult](https://docs.microsoft.com/pt-br/aspnet/core/web-api/action-return-types?view=aspnetcore-2.2#actionresultt-type). O ASP.NET Core serializa automaticamente o objeto em [JSON](https://www.json.org/) e grava o JSON no corpo da mensagem de resposta. O código de resposta para esse tipo de retorno é 200, supondo que não haja nenhuma exceção sem tratamento. As exceções sem tratamento são convertidas em erros 5xx.

Os tipos de retorno `ActionResult` podem representar uma ampla variedade de códigos de status HTTP. Por exemplo, `GetTodoItem` pode retornar dois valores de status diferentes:

- Se nenhum item corresponder à ID solicitada, o método retornará um código de erro 404 [NotFound](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.notfound).
- Caso contrário, o método retornará 200 com um corpo de resposta JSON. Retornar `item` resulta em uma resposta HTTP 200.

## Testar o método GetTodoItems

Agora vamos utilizar o Postman para testar os métodos implementados.

- Inicie o Postman.

- Desabilite a **Verificação do certificado SSL** (Como o certificado de desenvolvimento é auto-assinado, o Postman recusa a conexão)

  - Em **Arquivo > Configurações** (guia **Geral*), desabilite **Verificação do certificado SSL**.

     Aviso

    Habilite novamente a verificação do certificado SSL depois de testar o controlador.

- Crie uma solicitação.

  - Defina o método HTTP como **GET**.
  - Defina a URL de solicitação como `https://localhost:<port>/api/todo`. Por exemplo, `https://localhost:5001/api/todo`.

- Defina **Exibição de dois painéis** no Postman.

- Selecione **Enviar**.

![Postman com solicitação GET](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api/_static/2pv.png?view=aspnetcore-2.2)

## Adicionar um método Create

Adicione o seguinte método `PostTodoItem`:

```csharp
// POST: api/Todo
[HttpPost]
public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
{
    _context.TodoItems.Add(item);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
}
```

O código anterior é um método HTTP POST, conforme indicado pelo atributo [[HttpPost\]](https://docs.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.mvc.httppostattribute). O método salva no banco de dados os valores do item enviados no corpo da solicitação HTTP.

O método `CreatedAtAction`:

- retorna um código de status HTTP 201 em caso de êxito. HTTP 201 é a resposta padrão para um método HTTP POST que cria um novo recurso no servidor.

- Adiciona um cabeçalho `Location` à resposta. O cabeçalho `Location` especifica o URI do item de tarefas recém-criado. Para obter mais informações, confira [10.2.2 201 Criado](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html).

- Faz referência à ação `GetTodoItem` para criar o URI de `Location` do cabeçalho. A palavra-chave `nameof` do C# é usada para evitar o hard-coding do nome da ação, na chamada `CreatedAtAction`.

  ```csharp
  // GET: api/Todo/5
  [HttpGet("{id}")]
  public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
  {
      var todoItem = await _context.TodoItems.FindAsync(id);
  
      if (todoItem == null)
      {
          return NotFound();
      }
  
      return todoItem;
  }
  ```

### Testar o método PostTodoItem

- Execute novamente o projeto.

- No Postman, defina o método HTTP como `POST`.

- Selecione a guia **Corpo (body)**.

- Selecione o botão de opção **bruto (raw)**.

- Defina o tipo como **JSON (application/json)**.

- No corpo da solicitação, insira JSON para um item pendente:

  ```json
  {
    "name":"walk dog",
    "isComplete":true
  }
  ```

- Selecione **Enviar**.

  ![Postman com a solicitação Create](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api/_static/create.png?view=aspnetcore-2.2)

  Se você receber um erro 405 Método Não Permitido, é bem provável que o projeto não tenha sido parado e re-executado.

### Testar o URI do cabeçalho de local

- Selecione a guia **Cabeçalhos** no painel **Resposta**.

- Copie o valor do cabeçalho **Local**:

  ![Guia Cabeçalhos do console do Postman](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api/_static/pmc2.png?view=aspnetcore-2.2)

- Defina o método como GET.

- Cole o URI (por exemplo, `https://localhost:5001/api/Todo/2`)

- Selecione **Enviar**.

## Adicionar um método PutTodoItem

Adicione o seguinte método `PutTodoItem`:

```csharp
// PUT: api/Todo/5
[HttpPut("{id}")]
public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
{
    if (id != item.Id)
    {
        return BadRequest();
    }

    _context.Entry(item).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
}
```

`PutTodoItem` é semelhante a `PostTodoItem`, exceto pelo uso de HTTP PUT. A resposta é [204 (Sem conteúdo)](https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html). De acordo com a especificação de HTTP, uma solicitação PUT exige que o cliente envie a entidade inteira atualizada, não apenas as alterações. Para dar suporte a atualizações parciais, use [HTTP PATCH](https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.mvc.httppatchattribute).

Se você vir um erro ao chamar `PutTodoItem`, chame `GET` para garantir que existe um item com o ID referido no banco de dados.

### Testar o método PutTodoItem

Este exemplo usa um banco de dados em memória que deverá ser inicializado sempre que o aplicativo for iniciado (por isso a adição de um item padrão quando não há nenhum no banco).

Atualize o item pendente que tem a ID = 1 e defina seu nome como "feed fish":

```json
  {
    "ID":1,
    "name":"feed fish",
    "isComplete":true
  }
```

A seguinte imagem mostra a atualização do Postman:

![Console do Postman mostrando a resposta 204 (Sem conteúdo)](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/first-web-api/_static/pmcput.png?view=aspnetcore-2.2)

## Adicionar um método DeleteTodoItem

Adicione o seguinte método `DeleteTodoItem`:

```csharp
// DELETE: api/Todo/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTodoItem(long id)
{
    var todoItem = await _context.TodoItems.FindAsync(id);

    if (todoItem == null)
    {
        return NotFound();
    }

    _context.TodoItems.Remove(todoItem);
    await _context.SaveChangesAsync();

    return NoContent();
}
```

A resposta `DeleteTodoItem` é [204 (Sem conteúdo)](https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html).

### Testar o método DeleteTodoItem

Use o Postman para excluir um item pendente:

- Defina o método como `DELETE`.
- Defina o URI do objeto a ser excluído, por exemplo, `https://localhost:5001/api/todo/1`
- Selecione **Enviar**

O aplicativo de exemplo permite que você exclua todos os itens, mas quando o último item é excluído, um novo é criado pelo construtor de classe de modelo na próxima vez que a API é chamada.
