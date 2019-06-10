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

- [Swashbuckle.AspNetCore.Swagger](https://www.nuget.org/packages/Swashbuckle.AspNetCore.Swagger/): um modelo de objeto e um middleware do Swagger para expor objetos `SwaggerDocument` como pontos de extremidade JSON.
- [Swashbuckle.AspNetCore.SwaggerGen](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerGen/): um gerador do Swagger cria objetos `SwaggerDocument` diretamente de modelos, controladores e rotas. Normalmente, ele é combinado com o middleware de ponto de extremidade do Swagger para expor automaticamente o JSON do Swagger.
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

Inicie o aplicativo e navegue até `http://localhost:<port>/swagger/v1/swagger.json`. O documento gerado que descreve os pontos de extremidade é exibido conforme é mostrado na [Especificação do Swagger (swagger.json)](https://docs.microsoft.com/pt-br/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-2.2#swagger-specification-swaggerjson).

A interface do usuário do Swagger pode ser encontrada em `http://localhost:<port>/swagger`. Explore a API por meio da interface do usuário do Swagger e incorpore-a em outros programas.

