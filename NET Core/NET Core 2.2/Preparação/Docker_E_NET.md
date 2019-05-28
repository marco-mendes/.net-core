# Introdução ao .NET e ao Docker

O .NET Core pode ser facilmente executado em um contêiner do Docker. Os contêineres oferecem uma maneira leve de isolar seu aplicativo do restante do sistema do host, compartilhando apenas o kernel e usando os recursos fornecidos para seu aplicativo. 

Se você não estiver familiarizado com o Docker, é altamente recomendável ler a [documentação de visão geral](https://docs.docker.com/engine/docker-overview/) do Docker.

Para saber mais sobre como instalar o Docker, confira a página de download de [Docker Desktop: Community Edition](https://www.docker.com/products/docker-desktop).

## Noções básicas do Docker

Há alguns conceitos que você deve conhecer. O cliente do Docker tem um programa de interface de linha de comando que você usa para gerenciar imagens e contêineres. 

### Imagens

Uma imagem é uma coleção ordenada de alterações no sistema de arquivos que formam a base de um contêiner. A imagem não tem um estado e é somente leitura. Na maior parte das vezes, uma imagem é baseada em outra imagem, mas com alguma personalização. Por exemplo, quando você cria uma nova imagem para o aplicativo, você a baseará em uma imagem existente que já contenha o tempo de execução do .NET Core.

Como os contêineres são criados de imagens, as imagens têm um conjunto de parâmetros de execução (como um executável inicial) que são executados quando o contêiner é iniciado.

### Contêineres

Um contêiner é uma instância executável de uma imagem. Enquanto cria a imagem, você implanta seu aplicativo e dependências. Em seguida, vários contêineres podem ser instanciados, cada um isolado um do outro. Cada instância de contêiner tem seu próprio sistema de arquivos, memória e interface de rede.

### Registros

Registros de contêiner são uma coleção de repositórios de imagens. Você pode basear suas imagens em uma imagem de registro.Você pode criar contêineres diretamente em uma imagem em um registro. A [relação entre contêineres, imagens e registros do Docker](https://docs.microsoft.com/pt-br/dotnet/standard/microservices-architecture/container-docker-introduction/docker-containers-images-registries) é um conceito importante ao [arquitetar e compilar aplicativos ou microsserviços em contêineres](https://docs.microsoft.com/pt-br/dotnet/standard/microservices-architecture/architect-microservice-container-applications/index). Essa abordagem reduz bastante o tempo entre o desenvolvimento e a implantação.

O Docker tem um registro público disponível hospedado no [Hub do Docker](https://hub.docker.com/). As [imagens relacionadas ao .NET core](https://hub.docker.com/_/microsoft-dotnet-core/) estão listadas no Hub do Docker.

O MCR (Registro de Contêiner da Microsoft) é a fonte oficial de imagens de contêiner fornecidas pela Microsoft. O MCR baseia-se na CDN do Azure para fornecer imagens replicadas globalmente. No entanto, o MCR não tem um site voltado ao público e a principal maneira de aprender sobre imagens de contêiner fornecidas pela Microsoft é por meio das [páginas do Hub do Docker da Microsoft](https://hub.docker.com/_/microsoft-dotnet-core/).

### Dockerfile

Um **Dockerfile** é um arquivo que define um conjunto de instruções que cria uma imagem. Cada instrução no **Dockerfile** cria uma camada na imagem. Geralmente, quando você recria a imagem, somente as camadas que foram alteradas são recriadas. O **Dockerfile**pode ser distribuído para outras pessoas e permite que elas recriem para criar uma nova imagem da mesma maneira como você a criou. Embora isso permita distribuir as *instruções* sobre a criação de uma imagem, a principal forma de distribuir sua imagem é publicando-a em um registro.

## Imagens do .NET Core

As imagens oficiais do Docker do .NET Core são publicadas no MCR (Registro de Contêiner da Microsoft) e podem ser encontradas no [repositório do Hub do Docker do .NET Core da Microsoft](https://hub.docker.com/_/microsoft-dotnet-core/). Cada repositório contém imagens para diferentes combinações do .NET (SDK ou Runtime) e do sistema operacional que você pode usar.

A Microsoft fornece imagens personalizadas para cenários específicos. Por exemplo, o [repositório do ASP.NET Core](https://hub.docker.com/_/microsoft-dotnet-core-aspnet/) fornece imagens que são criadas para a execução de aplicativos ASP.NET Core na produção.