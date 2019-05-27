# Pré-requisitos para .NET Core no Windows



Este artigo mostra as versões de sistema operacional compatíveis para executar aplicativos do .NET Core no Windows. A versões do sistema operacional com suporte e as dependências a seguir são aplicáveis a três maneiras de desenvolver aplicativos .NET Core no Windows:

- [Linha de comando](https://docs.microsoft.com/pt-br/dotnet/core/tutorials/using-with-xplat-cli)
- [Visual Studio](https://www.visualstudio.com/downloads/?utm_medium=microsoft&utm_source=docs.microsoft.com&utm_campaign=button+cta&utm_content=download+vs2017)
- [Visual Studio Code](https://code.visualstudio.com/)

Além disso, se você estiver desenvolvendo no Windows usando o Visual Studio 2017, a seção [Pré-requisitos do Visual Studio 2017](https://docs.microsoft.com/pt-br/dotnet/core/windows-prerequisites?tabs=netcore2x#prerequisites-with-visual-studio-2017)mostra mais detalhes sobre as versões mínimas compatíveis para desenvolvimento do .NET Core.



## Versões do Windows com suporte pelo .NET Core

O .NET Core é compatível com as seguintes versões de:

- Windows 7 SP1

- Windows 8.1

- Atualização de Aniversário do Windows 10 (versão 1607) ou versões posteriores

- Windows Server 2008 R2 SP1 (Servidor Completo ou Server Core)

- Windows Server 2012 SP1 (Servidor Completo ou Server Core)

- Windows Server 2012 R2 (servidor completo ou Server Core)

- Windows Server 2016 ou versões posteriores (Servidor completo, Servidor Core ou Nano Server)

  

## Download do .NET Core

Para obter links de download e saber mais, consulte [Downloads do .NET](https://dotnet.microsoft.com/download) para baixar a versão mais recente ou [Arquivo de downloads do .NET](https://dotnet.microsoft.com/download/archives#dotnet-core) para obter versões mais antigas.



## Pré-requisitos do Visual Studio 2017

Você pode usar qualquer editor para desenvolver aplicativos .NET Core usando o SDK do .NET Core. O Visual Studio 2017 oferece um ambiente de desenvolvimento integrado para aplicativos .NET Core no Windows.

Leia mais sobre as alterações no Visual Studio 2017 nas [notas de versão](https://docs.microsoft.com/pt-br/visualstudio/releasenotes/vs2017-relnotes).

- [.NET Core 2.x](https://docs.microsoft.com/pt-br/dotnet/core/windows-prerequisites?tabs=netcore2x#tabpanel_CeZOj-G++Q_netcore2x)
- [.NET Core 1.x](https://docs.microsoft.com/pt-br/dotnet/core/windows-prerequisites?tabs=netcore2x#tabpanel_CeZOj-G++Q_netcore1x)

Para desenvolver aplicativos do .NET Core no Visual Studio 2017 usando o SDK do .NET Core 2.2:

1. [Baixe e instale o Visual Studio 2017 versão 15.9.0 ou superior](https://docs.microsoft.com/pt-br/visualstudio/install/install-visual-studio) com a carga de trabalho **Desenvolvimento de plataforma cruzada do .NET Core** (na seção **Outros conjuntos de ferramentas**) selecionada.

![Captura de tela da instalação do Visual Studio 2017 com a carga de trabalho "Desenvolvimento de plataforma cruzada do .NET Core" selecionada](https://docs.microsoft.com/pt-br/dotnet/core/media/windows-prerequisites/vs-2017-workloads.jpg)

Depois que o **desenvolvimento de plataforma cruzada do .NET Core** é instalado, o Visual Studio geralmente instala uma versão anterior do SDK do .NET Core. Por exemplo, o Visual Studio 2017 15.9 usa o SDK do .NET Core 2.1 por padrão após a instalação da carga de trabalho.

Para atualizar o Visual Studio para usar o SDK do .NET Core 2.2:

1. Instale o [SDK do .NET Core 2.2](https://dotnet.microsoft.com/download).
2. Se você quiser que seu projeto use o tempo de execução mais recente do .NET Core, redirecione projetos novos ou existentes do .NET Core para o .NET Core 2.2 usando as seguintes instruções:
   - No menu **Projeto**, escolha **Propriedades**.
   - No menu de seleção **Estrutura de Destino**, defina o valor como **.NET Core 2.2**.

![Captura de tela da propriedade de projeto do aplicativo do Visual Studio 2017 com o item de menu Estrutura de Destino “.NET Core 2.2” selecionado](https://docs.microsoft.com/pt-br/dotnet/core/media/windows-prerequisites/targeting-dotnet-core.jpg)

Quando o Visual Studio estiver configurado com o SDK do .NET Core 2.2, é possível fazer o seguintes:

- Abrir, compilar e executar projetos .NET Core 1.x e 2.x existentes.
- Redirecionar os projetos do .NET Core 1.x e 2.x para o .NET Core 2.2, compilar e executar.
- Criar novos projetos no .NET Core 2.2.