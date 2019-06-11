## Exercício de Documentação de APIs com o API Gateway

Ao consumir uma API Web, entender seus vários métodos pode ser um desafio para um desenvolvedor. O [Swagger](https://swagger.io/), também conhecido como [OpenAPI](https://www.openapis.org/), resolve o problema da geração de páginas de ajuda e de documentação úteis para APIs Web. Ele oferece benefícios, como documentação interativa, geração de SDK de cliente e capacidade de descoberta de API.

Neste artigo, são exibidas as implementações do .NET do Swagger [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore). O **Swashbuckle.AspNetCore** é um projeto de software livre para geração de documentos do Swagger para APIs Web ASP.NET Core.

Dica: O **NSwag** é outro projeto de software livre para geração de documentos do Swagger e a integração da [interface do usuário do Swagger](https://swagger.io/swagger-ui/) ou do [ReDoc](https://github.com/Rebilly/ReDoc) em APIs Web ASP.NET Core. Além disso, o NSwag oferece abordagens para gerar o código de cliente C# e TypeScript para sua API.

## O que é o Swagger / OpenAPI?

O Swagger é uma especificação independente de linguagem para descrever APIs [REST](https://en.wikipedia.org/wiki/Representational_state_transfer). O projeto de Swagger foi doado para a [Iniciativa OpenAPI](https://www.openapis.org/), na qual agora é chamado de OpenAPI. Ambos os nomes são intercambiáveis, mas OpenAPI é o preferencial.Ele permite que computadores e pessoas entendam os recursos de um serviço sem nenhum acesso direto à implementação (código-fonte, acesso à rede, documentação). Uma meta é minimizar a quantidade de trabalho necessário para conectar-se a serviços desassociados. Outra meta é reduzir o período de tempo necessário para documentar um serviço com precisão.

## Especificação do Swagger (swagger.json)

O ponto central para o fluxo do Swagger é a especificação do Swagger que é, por padrão, um documento chamado *swagger.json*. Ele é gerado por ferramentas Swagger (ou por implementações de terceiros) com base no seu serviço. Ele descreve os recursos da API e como acessá-lo com HTTP. Ele gera a interface do usuário do Swagger e é usado pela cadeia de ferramentas para habilitar a geração e a descoberta de código de cliente. 

Aqui está um exemplo de uma especificação do Swagger, resumida:

```json
{
   "swagger": "2.0",
   "info": {
       "version": "v1",
       "title": "API V1"
   },
   "basePath": "/",
   "paths": {
       "/api/Todo": {
           "get": {
               "tags": [
                   "Todo"
               ],
               "operationId": "ApiTodoGet",
               "consumes": [],
               "produces": [
                   "text/plain",
                   "application/json",
                   "text/json"
               ],
               "responses": {
                   "200": {
                       "description": "Success",
                       "schema": {
                           "type": "array",
                           "items": {
                               "$ref": "#/definitions/TodoItem"
                           }
                       }
                   }
                }
           },
           "post": {
               ...
           }
       },
       "/api/Todo/{id}": {
           "get": {
               ...
           },
           "put": {
               ...
           },
           "delete": {
               ...
   },
   "definitions": {
       "TodoItem": {
           "type": "object",
            "properties": {
                "id": {
                    "format": "int64",
                    "type": "integer"
                },
                "name": {
                    "type": "string"
                },
                "isComplete": {
                    "default": false,
                    "type": "boolean"
                }
            }
       }
   },
   "securityDefinitions": {}
}
```

## Interface do usuário do Swagger

A [Interface do usuário do Swagger](https://swagger.io/swagger-ui/) oferece uma interface do usuário baseada na Web que conta com informações sobre o serviço, usando a especificação do Swagger gerada. O Swashbuckle e o NSwag incluem uma versão incorporada da interface do usuário do Swagger, para que ele possa ser hospedado em seu aplicativo ASP.NET Core usando uma chamada de registro de middleware. A interface do usuário da Web tem esta aparência:

![Interface do usuário do Swagger](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/swagger-ui.png?view=aspnetcore-2.2)

Todo método de ação pública nos controladores pode ser testado da interface do usuário. Você pode clicar em um nome de método para expandir a seção e depois clicar no botão Try it Out.

![Teste GET de Swagger de exemplo](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/get-try-it-out.png?view=aspnetcore-2.2)

# Introdução ao Swashbuckle e ao ASP.NET Core

Há três componentes principais bo Swashbuckle:

- [Swashbuckle.AspNetCore.Swagger](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/): um modelo de objeto e um middleware do Swagger para expor objetos `SwaggerDocument` como rotas JSON.
- [Swashbuckle.AspNetCore.SwaggerGen](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerGen/): um gerador do Swagger cria objetos `SwaggerDocument` diretamente de modelos, controladores e rotas. Normalmente, ele é combinado com o middleware da rota do Swagger para expor automaticamente o JSON do Swagger.
- [Swashbuckle.AspNetCore.SwaggerUI](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerUI/): uma versão incorporada da ferramenta de interface do usuário do Swagger. Ele interpreta o JSON do Swagger para criar uma experiência avançada e personalizável para descrever a funcionalidade da API Web. Ela inclui o agente de teste interno para os métodos públicos.

Vamos gerar um projeto mínimo de API com o seguinte comando,

```sh
dotnet new api -o TodoAPI
```

Navegue para o diretório TodoAPI e adicione a dependência do pacote Swashbuckle

```sh
dotnet add TodoApi.csproj package Swashbuckle.AspNetCore
```

Iremos habilitar o Swagger editando o arquivo Startup.cs dentro do seu projeto.

Primeiramente, importe o pacote do Swagger com o código abaixo:

```c#
using Swashbuckle.AspNetCore.Swagger;
```

Depois, modifique o método ConfigureServices conforme código abaixo.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<TodoContext>(opt =>
        opt.UseInMemoryDatabase("TodoList"));
    services.AddMvc()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

    // Register the Swagger generator, defining 1 or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Info { Title = "Minha Primeira API", Version = "v1" });
    });
}
```

No método `Startup.Configure`, habilite o middleware para atender ao documento JSON gerado e à interface do usuário do Swagger:

```C#
public void Configure(IApplicationBuilder app)
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseMvc();
}
```



A chamada do método `UseSwaggerUI` precedente habilita o [middleware de arquivos estáticos](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/static-files?view=aspnetcore-2.2). Se você estiver direcionando ao .NET Framework ou ao .NET Core 1.x, adicione o pacote NuGet [Microsoft.AspNetCore.StaticFiles](https://www.nuget.org/packages/Microsoft.AspNetCore.StaticFiles/) ao projeto.

Inicie o aplicativo e navegue até `http://localhost:<port>/swagger/v1/swagger.json`. O documento gerado que descreve as rotas é exibido conforme é mostrado na [Especificação do Swagger (swagger.json)] apresentado no começo desse tutorial.

A interface do usuário do Swagger pode ser encontrada em `http://localhost:<port>/swagger`. Explore a API por meio da interface do usuário do Swagger e incorpore-a em outros programas.

## Personalização e Extensão da Interface do Swagger

O Swagger fornece opções para documentar o modelo de objeto e personalizar a interface do usuário para corresponder ao seu tema.

### Descrição e informações da API

A ação de configuração passada para o método `AddSwaggerGen` adiciona informações como o autor, a licença e a descrição. Para isso, adicione o trecho de código abaixo no método ConfigureServices. 

```c#
// Register the Swagger generator, defining 1 or more Swagger documents
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Info
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "Um exemoplo simples de ASP.NET Core Web API",
        TermsOfService = "None",
        Contact = new Contact
        {
            Name = "Shayne Boyer",
            Email = string.Empty,
            Url = "https://twitter.com/spboyer"
        },
        License = new License
        {
            Name = "Use under LICX",
            Url = "https://example.com/license"
        }
    });
});
```



## Enriquecimento da documentação para as rotas de API

Adicionar comentários de barra tripla a uma ação aprimora a interface do usuário do Swagger adicionando a descrição ao cabeçalho da seção. Adicione um elemento [](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/xmldoc/summary) acima da ação `Delete`conforme código abaixo.

```c#
/// <summary>
/// Deletes a specific TodoItem.
/// </summary>
/// <param name="id"></param>        
[HttpDelete("{id}")]
public IActionResult Delete(long id)
{
    var todo = _context.TodoItems.Find(id);

    if (todo == null)
    {
        return NotFound();
    }

    _context.TodoItems.Remove(todo);
    _context.SaveChanges();

    return NoContent();
}
```

A interface do usuário do Swagger exibe o texto interno do elemento `<summary>` do código anterior:

![A interface do usuário do Swagger, mostrando o comentário XML 'Exclui um TodoItem específico'.](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/triple-slash-comments.png?view=aspnetcore-2.2)



A interface do usuário é controlada pelo esquema JSON gerado:

```json
"delete": {
    "tags": [
        "Todo"
    ],
    "summary": "Deletes a specific TodoItem.",
    "operationId": "ApiTodoByIdDelete",
    "consumes": [],
    "produces": [],
    "parameters": [
        {
            "name": "id",
            "in": "path",
            "description": "",
            "required": true,
            "type": "integer",
            "format": "int64"
        }
    ],
    "responses": {
        "200": {
            "description": "Success"
        }
    }
}
```

Adicione um elemento [remarks](https://docs.microsoft.com/pt-br/dotnet/csharp/programming-guide/xmldoc/remarks) na documentação do método da ação `Create`. Ele complementa as informações especificadas no elemento `<summary>` e fornece uma interface de usuário do Swagger mais robusta. O conteúdo do elemento `<remarks>` pode consistir em texto, JSON ou XML.

```c#
/// <summary>
/// Creates a TodoItem.
/// </summary>
/// <remarks>
/// Sample request:
///
///     POST /Todo
///     {
///        "id": 1,
///        "name": "Item1",
///        "isComplete": true
///     }
///
/// </remarks>
/// <param name="item"></param>
/// <returns>A newly created TodoItem</returns>
/// <response code="201">Returns the newly created item</response>
/// <response code="400">If the item is null</response>            
[HttpPost]
[ProducesResponseType(201)]
[ProducesResponseType(400)]
public ActionResult<TodoItem> Create(TodoItem item)
{
    _context.TodoItems.Add(item);
    _context.SaveChanges();

    return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
}
```

Observe os aprimoramentos da interface do usuário com esses comentários adicionais.

![Interface do usuário do Swagger com comentários adicionais mostrados](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/xml-comments-extended.png?view=aspnetcore-2.2)



Adicione o atributo `[Produces("application/json")]` ao controlador da API. Sua finalidade é declarar que as ações do controlador permitem o tipo de conteúdo de resposta *application/json*:

```c#
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;
```



A lista suspensa **Tipo de Conteúdo de Resposta** seleciona esse tipo de conteúdo como o padrão para ações GET do controlador:

![Interface do usuário do Swagger com o tipo de conteúdo de resposta padrão](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/json-response-content-type.png?view=aspnetcore-2.2)

À medida que aumenta o uso de anotações de dados na API Web, a interface do usuário e as páginas de ajuda da API se tornam mais descritivas e úteis.

Os desenvolvedores que usam uma API Web estão mais preocupados com o que é retornado—, especificamente, os tipos de resposta e os códigos de erro (se eles não forem padrão). Os tipos de resposta e os códigos de erro são indicados nos comentários XML e nas anotações de dados.

A ação `Create` retorna um código de status HTTP 201 em caso de sucesso. Um código de status HTTP 400 é retornado quando o corpo da solicitação postada é nulo. Sem a documentação adequada na interface do usuário do Swagger, o consumidor não tem conhecimento desses resultados esperados. Corrija esse problema adicionando as linhas realçadas no exemplo a seguir:

```c#
/// <summary>
/// Creates a TodoItem.
/// </summary>
/// <remarks>
/// Sample request:
///
///     POST /Todo
///     {
///        "id": 1,
///        "name": "Item1",
///        "isComplete": true
///     }
///
/// </remarks>
/// <param name="item"></param>
/// <returns>A newly created TodoItem</returns>
/// <response code="201">Returns the newly created item</response>
/// <response code="400">If the item is null</response>            
[HttpPost]
[ProducesResponseType(201)]
[ProducesResponseType(400)]
public ActionResult<TodoItem> Create(TodoItem item)
{
    _context.TodoItems.Add(item);
    _context.SaveChanges();

    return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
}
```



A interface do usuário do Swagger agora documenta claramente os códigos de resposta HTTP esperados:

![A interface do usuário do Swagger mostra a descrição da classe de resposta POST, 'Retorna o item de tarefa pendente recém-criado' e '400 – se o item for nulo' para o código de status e o motivo em Mensagens de Resposta](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger/_static/data-annotations-response-types.png?view=aspnetcore-2.2)

Esses são alguns exemplos de personalização. Você pode explorar e conhecer muito mais exemplos [aqui](https://codingsight.com/swashbuckle-swagger-configuration-for-webapi/).