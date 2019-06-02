### Tópicos de aprofundamento



#### Sobre dotnet publish

Observe que os comandos e as etapas mostradas neste tutorial para executar o aplicativo são usadas somente durante o tempo de desenvolvimento. Quando estiver pronto para implantar o aplicativo, você deve dar uma olhada nas diferentes [estratégias de implantação](https://docs.microsoft.com/pt-br/dotnet/core/deploying/index) para aplicativos .NET Core e no comando [`dotnet publish`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-publish).

#### Sobre compilação auto contida

Em um cenário avançado, é possível compilar o aplicativo como um pacote completo, que pode ser implantado e executado em um ambiente que não possui o .NET Core instalado. Consulte [Implantação do .NET Core Application](https://docs.microsoft.com/pt-br/dotnet/core/deploying/index) para obter mais informações.

#### Sobre docker-compose

O Docker permite ainda a criação de uma aplicação que contém vários conteineres orquestrados operando juntos, como por exemplo um container contendo uma aplicação web e outro contendo um banco de dados. Para isso, é utilizado o docker-compose. Mais informações sobre podem ser encontradas [aqui](https://docs.docker.com/compose/overview/), [aqui](https://imasters.com.br/banco-de-dados/docker-compose-o-que-e-para-que-serve-o-que-come) e [aqui](https://www.mundodocker.com.br/docker-compose/).

#### Sobre códigos de retorno HTTP

Os códigos de status das respostas HTTP indicam se uma requisição HTTP foi corretamente concluída. As respostas são agrupadas em cinco classes: respostas de informação, respostas de sucesso, redirecionamentos, erros do cliente e erros do servidor. Cada número é uma convenção e representa uma resposta específica, que pode ser conferida [aqui](https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status).

#### Sobre tipos de testes

- Testes unitários ou de unidade
  - *Testes unitários são executados sobre a menor parte testável de um software a fim de determinar se ela se comporta como o esperado.*
- Testes de integração
  - *Um teste de integração verifica a comunicação e as interações entre os componentes a fim de identificar principalmente erros de interface e interação.*
- Teste de componente
  - *Um teste de componente limita o escopo da execução do software a uma porção que será testada, manipulando o estado da aplicação e utilizando testes para verificar o correto e esperado funcionamento de um componente.*
- Teste de contrato
  - *Um teste de integração por contrato verifica se chamadas por seviços externos têm a resposta esperada da aplicação.*
- Testes ponta-a-ponta
  - *Testes de pontat-a-ponta verificam se o sistema atinge as especificações externas e funciona corretamente como um todo, do começo ao fim.* 

Mais informações sobre cada tipo de teste e sua definição pode ser encontradas [aqui](https://martinfowler.com/articles/microservice-testing/).

#### Sobre Identificação JWT com ASP.NET Core

A identificação de um usuário no sistema pode ser feita de várias maneiras. Seja integrada ao Windows, por uma página com um formulário da aplicação ou até por chamadas à APIs. Cada uma traz consigo uma série de restrições, principalmente relacionadas ao ambiente em que a aplicação é executada.

Com a adoção cada vez maior de microsserviços, se tornou comum encontrar interfaces API em várias aplicações atuais. A grande maioria delas utiliza Tokens de sessão, que permite autenticar o usuário através de uma string criptografada que acompanha o header de cada request. E claro, há sempre um exemplo de implantação, como mostrado por Renato Groffe, em seu post no [Medium](https://medium.com/@renato.groffe/jwt-asp-net-core-2-2-exemplo-de-implementação-3e10058c1a73) e seu repositório no [GitHub](https://github.com/renatogroffe/ASPNETCore2.2_JWT-Identity).







