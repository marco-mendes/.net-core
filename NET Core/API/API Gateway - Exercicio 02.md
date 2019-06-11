### Exercício de API Gateway

Nesse exercício você irá usar os conhecimentos aprendidos nos tutoriais anteriores de API para tratar dois aspectos de construção de APIs.

QoS - Qualidade do serviço

---

Você pode criar aspectos de qualidade de serviço para a chamada de APIs com o padrão de projeto **CircuitBreaker**. 

O que é um Circuit Breaker (Disjuntor)?

---

É comum que os sistemas de software façam chamadas remotas para o software em execução em diferentes processos, provavelmente em máquinas diferentes em uma rede. Uma das grandes diferenças entre chamadas na memória e chamadas remotas é que as chamadas remotas podem falhar ou travar sem uma resposta até que um limite de tempo limite seja atingido. O que é pior se você tiver muitos chamadores em um fornecedor que não responde, então você pode ficar sem recursos críticos, levando a falhas em cascata em vários sistemas. Em seu excelente livro, Release It, Michael Nygard popularizou o padrão Disjuntor para evitar esse tipo de cascata catastrófica.

A ideia básica por trás do disjuntor é muito simples. Você envolve uma chamada de função protegida em um objeto de disjuntor, que monitora falhas. Uma vez que as falhas atinjam um certo limite, o disjuntor desarma, e todas as outras chamadas para o disjuntor retornam com um erro, sem que a chamada protegida seja feita. Normalmente, você também vai querer algum tipo de alerta de monitor caso o disjuntor desarme.

O Disjuntor no Ocelot

---

O Ocelot implementa nativamente o Circuit Breaker.

Por exemplo, o API Gateway Ocelot pode abrir o circuito do disjuntor por um segundo se uma requisição demora mais que 5 segundos com a seguinte parametrização.

```json
{
  "ReRoutes": [
    {
      "DownstreamPathTemplate": "/api/values",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 9001
        }
      ],
      "UpstreamPathTemplate": "/customers",
      "UpstreamHttpMethod": [ "Get" ],
      "QoSOptions": {
        "ExceptionsAllowedBeforeBreaking":2,
        "DurationOfBreak":5000,
        "TimeoutValue":2000
      }
    },
    ...
```

Você pode estudar essa parametrização em mais detalhes com as informações encontradas [aqui](https://ocelot.readthedocs.io/en/latest/features/qualityofservice.html)

**A partir dessas informações, implemente o padrão Circuit Breaker no seu código realizar no Exercicio 01.**

Controle de LImite de Chamadas

---

Uma outra funcionalidade interessante de API Gateways é o controle do limite de chamadas. Isso pode ser configurado no Ocelot através da seguinte configuração.

```json
{
    "ReRoutes": [
        {
            "DownstreamPathTemplate": "/api/values",
            "DownstreamScheme": "http",
            "DownstreamHostAndPorts": [
                {
                    "Host": "localhost",
                    "Port": 9001
                }
            ],
            "UpstreamPathTemplate": "/customers",
            "UpstreamHttpMethod": [ "Get" ],
            "RateLimitOptions": {
                "ClientWhitelist": [],
                "EnableRateLimiting": true,
                "Period": "1h",
                "PeriodTimespan": 100,
                "Limit": 1
            }
        },
      ...
```

Você pode examinar uma documentação mais extensa de como realizar essas parametrização [aqui](https://ocelot.readthedocs.io/en/latest/features/ratelimiting.html) e [aqui](https://www.c-sharpcorner.com/article/building-api-gateway-using-ocelot-in-asp-net-core-rate-limiting-part-four/).

A partir dessas informações, implemente esse padrão no seu código fonte.

---

Balanceamento de Carga

O Ocelot pode balancear a carga entre os serviços disponíveis para cada ReRoute. Isso significa que você pode dimensionar seus serviços downstream e o Ocelot pode usá-los de forma eficaz. 

Por exemplo, a configuração abaixo especifica que a rota `post\{postId}` será redirecionada para dois servidores com IPs 10.0.1.10 e 10.0.1.1 em um mecanismo de balanceamento de carga.

```json
{
    "DownstreamPathTemplate": "/api/posts/{postId}",
    "DownstreamScheme": "https",
    "DownstreamHostAndPorts": [
            {
                "Host": "10.0.1.10",
                "Port": 5000,
            },
            {
                "Host": "10.0.1.11",
                "Port": 5000,
            }
        ],
    "UpstreamPathTemplate": "/posts/{postId}",
    "LoadBalancerOptions": {
        "Type": "LeastConnection"
    },
    "UpstreamHttpMethod": [ "Put", "Delete" ]
}
```

Leia as informações de parametrização [aqui disponíveis](https://ocelot.readthedocs.io/en/latest/features/loadbalancer.html) e incorpore essa capacidade na sua aplicação.

