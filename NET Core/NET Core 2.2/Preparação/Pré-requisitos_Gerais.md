## Pré -requisitos para o módulo NET Core 2.2

Os seguintes programas são requeridos para a execução completa de todos os passos do módulo.

- [Docker](https://docs.docker.com/docker-for-windows/install/)

- [Visual Studio Code (ou editor de sua preferência)](https://code.visualstudio.com/)

- [SDK do .NET Core 2.2](https://dotnet.microsoft.com/download/archives)

- [GIT Client](https://git-scm.com/download/win)

  

Além dos programas, para execução devida dos laboratórios do docker, são necessárias as imagens:

- microsoft/dotnet:2.2-sdk

- mcr.microsoft.com/dotnet/core/runtime:2.2

Que devido ao seu tamanho, devem ser baixadas previamente para manter a fluidez. Elas podem ser baixadas através dos comandos:

```bash

docker pull mcr.microsoft.com/dotnet/core/runtime:2.2
docker pull microsoft/dotnet:2.2-sdk

```

