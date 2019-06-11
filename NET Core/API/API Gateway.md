## Exercício de API Gateway

O API Gateway funciona como uma porta de entrada para o clientes dos serviços de um sistema. No lugar de invocar um serviço (ou microsserviços) diretamente, os clientes chamam a API Gateway, que irá redirecionar a solicitação para o microsserviço apropriado. Quando o serviço retornar à solicitação, o API Gateway realizar o retorno para o cliente.

Ou seja, ele funciona como uma camada intermediária entre os clientes e os serviços. Como todas as solicitações irão passar por ele, o gateway pode modificar as solicitações recebidas e retornadas, o que nos fornece algumas vantagens:

- Os serviços podem ser modificados sem se preocupar com os clientes;
- Os serviços podem se comunicar utilizando qualquer tipo de protocolo. O importante é o gateway implementar um protocolo que seja compreendido pelos clientes.
- O gateway pode implementar recursos que não impactam nos microsserviços, como autenticação, logging, SSL, balanceamento de carga, etc.

Veja aqui um exemplo conceitual de um projeto disponibilizado pela Microsoft, que usa um API Gateway como porta de entrada das aplicações Web para consumo de um conjunto de microsserviços.

[![img](https://user-images.githubusercontent.com/1712635/38758862-d4b42498-3f27-11e8-8dad-db60b0fa05d3.png)](https://user-images.githubusercontent.com/1712635/38758862-d4b42498-3f27-11e8-8dad-db60b0fa05d3.png)

O código fonte desse projeto disponibilizado [aqui](https://github.com/dotnet-architecture/eShopOnContainers). 

## API Gateway como um Padrão de Desenho

## Ocelot

O Ocelot é uma biblioteca que permite criar um API Gateway com o ASP.NET. Ele possui uma grande gama de funcionalidades, como:

- Agregação de solicitações;
- Roteamento;
- Autenticação;
- Autorização;
- Cache;
- Balanceamento de carga;
- Log;
- Trace;
- WebSockets;
- QoS
- Limitação de uso (Rate Limiting)
- Transformação de cabeçalho;
- Correlação de IDs;
- Suporte ao Docker;
- Suporte ao Kubernetes;
- Suporte ao Service Fabric.

Mesmo que seja voltado para aplicações .NET que estejam implementando uma arquitetura de microsserviços, o Ocelot pode ser utilizado como API Gateway de qualquer tipo de sistema que implemente esta arquitetura.

## Criação do Projeto de Exemplo

Para exemplificar o uso do Ocelot, vamos criar dois microsserviços, que serão apenas aplicações ASP.NET Web API. Uma se chamará `Pedido` e a outra `Catalogo`. A única diferença dessas aplicações para a padrão criada pelo .NET é uma modificação no controller `Values`:

```shell
dotnet new webapi -o Pedido.API
```

```shell
dotnet new webapi -o Catalogo.API
```

Modifique os métodos GET em cada um dos projetos para identificar cada um dos microsserviços.

```c#
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    return new string[] { "Item 1 do serviço de pedido", "Item 2 do serviço de pedido" };
}
```

```c#
[HttpGet]
public ActionResult<IEnumerable<string>> Get()
{
    return new string[] { "Produto 1 do serviço de catálogo", "Produto 2 do serviço do catálogo" };
}
```

## Criando o API Gateway

Como dito , o Ocelot permite configurar uma aplicação ASP.NET para que se comporte como API Gateway. Assim, para criá-lo inicialmente é necessário criar uma aplicação ASP.NET.

Como a aplicação funcionará apenas como API Gateway, podemos criar uma aplicação “vazia”.

```shell
dotnet new web -o Gateway
```

Este é o procedimento recomendado, mas é possível definir o Ocelot em qualquer tipo de aplicação ASP.NET.

Após a criação da aplicação é necessário adicionar a referência do Ocelot:

```shell
dotnet add package Ocelot
```

As configurações do Ocelot devem ser definidas em um arquivo JSON. Assim, vamos alterar a classe `Program` para que carregue este arquivo no início da execução da aplicação:

```csharp
public class Program
{
    //Código omitido

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ic => ic.AddJsonFile("configuration.json"))
            .UseStartup<Startup>();
}
```



Veremos este arquivo de configuração à frente. Antes, iremos alterar a classe Startup. Nela iremos habilitar o serviço e o middleware do Ocelot.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

// Imports devido aos Ocelot
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Gateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(_configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOcelot().Wait();
        }
    }
}
```



É importante que esta sempre seja a última linha do método `Configure`. Isso porque o middleware do Ocelot é terminal. Ou seja, ele não invoca nenhum outro middleware do pipeline. Caso a solicitação realizada não seja encontrada nas configurações do Ocelot, ele irá gerar um erro 404.

## Configurando o API Gateway

O arquivo de configuração do Ocelot é composto de dois atributos:

```json
{
    "ReRoutes": [],
    "GlobalConfiguration": {}
}
```

Em `ReRoutes` definimos como funcionará o sistema de redirecionamento da API Gateway. Já no `GlobalConfiguration` definimos configurações globais que sobrescrevem configurações do `ReRoutes`.

No `ReRoutes` é possível definir uma série de funcionalidades, mas os pontos mais importantes são:

- **DownstreamPathTemplate**: Define a URL que será utilizada na criação da solicitação para o microsserviço;
- **DownstreamScheme**: Define o scheme utilizado na solicitação para o microsserviço;
- **DownstreamHostAndPorts**: Define o `Host` e a porta (`Port`) utilizada na solicitação para o microsserviço;
- **UpstreamPathTemplate**: Define a URL que o Ocelot irá utilizar para indicar que deve ser chamado o microsserviço definido nos atributos **Downstream**
- **UpstreamHttpMethod**: Define os métodos HTTP aceitos;

Com isso em mente, podemos definir a seguinte configuração para a nossa API Gateway:

```json
{
    "ReRoutes": [
      {
        "DownstreamPathTemplate": "/",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
          {
            "Host": "127.0.0.1",
            "Port": 7000
          }
        ],
        "UpstreamPathTemplate": "/pedido/",
        "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ]
      },
      {
        "DownstreamPathTemplate": "/{everything}",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
          {
            "Host": "127.0.0.1",
            "Port": 7000
          }
        ],
        "UpstreamPathTemplate": "/pedido/{everything}",
        "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ]
      },
      {
        "DownstreamPathTemplate": "/",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
          {
            "Host": "127.0.0.1",
            "Port": 8000
          }
        ],
        "UpstreamPathTemplate": "/catalogo/",
        "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ]
      },
      {
        "DownstreamPathTemplate": "/{everything}",
        "DownstreamScheme": "http",
        "DownstreamHostAndPorts": [
          {
            "Host": "127.0.0.1",
            "Port": 8000
          }
        ],
        "UpstreamPathTemplate": "/catalogo/{everything}",
        "UpstreamHttpMethod": [ "Get", "Post", "Put", "Delete", "Options" ]
      }
    ],
    "GlobalConfiguration": { }
  }
```

Crie um arquivo chamado configuration.json na raiz do seu projeto e copie o conteúdo acima para esse arquivo.

## Execução dos Projetos

Execute o projeto de gateway através do comando  `dotnet run` na raiz desse projeto.

Você deve agora executar os projetos criados anteriormente (Pedido.API e Catalogo.API). Antes de executar-lo, devemos mudar as portas onde cada um desses projetos irá rodar.  

Para isso, vá no arquivo launchSettings,json do projeto Pedido.API e mude a porta desse serviço para 7000 conforme código abaixo.

```json
    "Pedido.API": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "api/values",
      "applicationUrl": "https://localhost:7001;http://localhost:7000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
```



Agora vá no arquivo launchSettings,json do projeto Catalogo.API e mude a porta desse serviço para 8000 conforme código abaixo.

```json
    "Catalogo.API": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "api/values",
      "applicationUrl": "https://localhost:8001;http://localhost:8000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
```

Rode o código invocando cada microsserviço da seguinte forma.

`http://localhost:5000/pedido/api/values` 

`http://localhost:5000/catalogo/api/values`

Os resultados devem ser, respectivamente:

{ "Item 1 do serviço de pedido", "Item 2 do serviço de pedido" } e

{ "Produto 1 do serviço de catálogo", "Produto 2 do serviço do catálogo" };



