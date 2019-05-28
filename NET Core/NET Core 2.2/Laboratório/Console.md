Este tópico mostra como começar a desenvolver aplicativos de plataforma cruzada no computador usando as ferramentas da CLI do .NET Core.

Se não estiver familiarizado com o conjunto de ferramentas da CLI do .NET Core, leia a [Visão geral do SDK do .NET Core](https://docs.microsoft.com/pt-br/dotnet/core/tools/index).

## Pré-requisitos

- [SDK do .NET Core 2.2](https://www.microsoft.com/net/download/core).
- Um editor de texto ou de código de sua escolha. Recomendamos o VSCOde, que pode ser baixado daqui https://code.visualstudio.com

## Olá, Aplicativo de Console.

Você pode [exibir ou baixar o código de exemplo](https://github.com/dotnet/samples/tree/master/core/console-apps/HelloMsBuild) do repositório dotnet/samples do GitHub.

Abra um prompt de comando e crie uma pasta chamada *PrimeiroConsole*. Navegue até a pasta que você criou e digite o seguinte:

```console
dotnet new console
dotnet run
```

Vejamos um breve passo a passo:

1. `dotnet new console`

   [`dotnet new`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new) cria um arquivo de projeto `PrimeiroConsole.csproj` atualizado com as dependências necessárias para criar um aplicativo de console. Ele também cria um `Program.cs`, um arquivo básico que contém o ponto de entrada para o aplicativo.

   `PrimeiroConsole.csproj`:

   ```
   <Project Sdk="Microsoft.NET.Sdk">
   
     <PropertyGroup>
       <OutputType>Exe</OutputType>
       <TargetFramework>netcoreapp2.2</TargetFramework>
     </PropertyGroup>
   
   </Project>
   ```

   O arquivo de projeto especifica tudo o que é necessário para restaurar as dependências e compilar o programa.

   - A marca `OutputType` especifica que estamos copilando um executável, em outras palavras, um aplicativo de console.
   - A marca `TargetFramework` especifica qual implementação do .NET estamos direcionando. Em um cenário avançado, é possível especificar várias estruturas de destino e criar para todas elas em uma única operação. Neste tutorial, veremos apenas a compilação para .NET Core 2.2.

   `Program.cs`:

   ```csharp
   using System;
   
   namespace PrimeiroConsole
   {
       class Program
       {
           static void Main(string[] args)
           {
               Console.WriteLine("Hello World!");
           }
       }
   }
   ```

   O programa é iniciado pelo `using System`, que significa "colocar tudo no namespace `System` no escopo para este arquivo". O namespace `System` inclui construções básicas, como `string` ou tipos numéricos.

   Em seguida, definimos um namespace chamado `PrimeiroConsole`. Você pode alterar isso de acordo com a sua vontade. Uma classe chamada `Program` é definida dentro desse namespace, com um método `Main` que recebe como argumento uma cadeia de caracteres. Esses parâmetros contêm a lista de argumentos passados quando o programa compilado é executado. Por enquanto, a única coisa que o programa faz é gravar: "Hello World!" no console. Posteriormente, faremos alterações no código que farão uso desses parâmetros.

    **Observação**

   Começando com o SDK do .NET Core 2.0, não é necessário executar [`dotnet restore`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-restore), pois ele é executado implicitamente por todos os comandos que exigem uma restauração, como `dotnet new`, `dotnet build` e `dotnet run`. Ainda é um comando válido em determinados cenários em que realizar uma restauração explícita faz sentido, como [builds de integração contínua no Azure DevOps Services](https://docs.microsoft.com/azure/devops/build-release/apps/aspnet/build-aspnet-core) ou em sistemas de build que precisam controlar explicitamente o horário em que a restauração ocorrerá.

   `dotnet new` chama [`dotnet restore`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-restore) implicitamente. `dotnet restore` chama o [NuGet](https://www.nuget.org/) (gerenciador de pacotes do .NET) para restaurar a árvore de dependências. O NuGet analisa o arquivo *PrimeiroConsole.csproj*, baixa as dependências definidas no arquivo (ou captura-as de um cache no computador) e grava o arquivo *obj/project.assets.json*, que é necessário para compilar e executar a amostra.

2. `dotnet run`

   [`dotnet run`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-run) chama [`dotnet build`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-build) para garantir que os destinos de build foram criados e então chama `dotnet <assembly.dll>` para executar o aplicativo de destino.

   ```console
   $ dotnet run
   Hello World!
   ```

   Como alternativa, também é possível pode executar [`dotnet build`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-build) para compilar o código sem executar os aplicativos de console de compilação. Isso resulta em um aplicativo compilado como um arquivo DLL que pode ser executado com `dotnet bin\Debug\netcoreapp2.2\PrimeiroConsole.dll` no Windows (caso esteja em outro sistema, use `/`). Também é possível especificar argumentos para o aplicativo, como você verá adiante no tópico.

   ```console
   $ dotnet bin\Debug\netcoreapp2.2\PrimeiroConsole.dll
   Hello World!
   ```



### Ampliando o programa

Vamos alterar o programa um pouco, explorando um pouco mais sobre as aplicações console. E para escolhar um tópico divertido e infinito, que tal Números de Fibonacci? Então, vamos implementar o algoritmo e também fazer uso do argumento para saudar a pessoa que executa o aplicativo.

1. Substitua o conteúdo do arquivo *Program.cs* pelo seguinte código:

   ```csharp
   using System;
   
   namespace PrimeiroConsole
   {
       class Program
       {
           static void Main(string[] args)
           {
               if (args.Length > 0)
               {
                   Console.WriteLine($"Ola {args[0]}!");
               }
               else
               {
                   Console.WriteLine("Ola!");
               }
   
               Console.WriteLine("Numeros Fibonacci 1-15:");
   
               for (int i = 0; i < 15; i++)
               {
                   Console.WriteLine($"{i + 1}: {FibonacciNumber(i)}");
               }
           }
   
           static int FibonacciNumber(int n)
           {
               int a = 0;
               int b = 1;
               int tmp;
   
               for (int i = 0; i < n; i++)
               {
                   tmp = a;
                   a = b;
                   b += tmp;
               }
   
               return a;
           }
   
       }
   }
   ```

2. Execute [`dotnet build`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-build) para compilar as alterações.

3. Execute o programa passando um parâmetro para o aplicativo:

   ```console
   $ dotnet run -- John
   Ola John!
   Numeros Fibonacci 1-15:
   1: 0
   2: 1
   3: 1
   4: 2
   5: 3
   6: 5
   7: 8
   8: 13
   9: 21
   10: 34
   11: 55
   12: 89
   13: 144
   14: 233
   15: 377
   ```

E pronto. Você pode ampliar `Program.cs` como desejar.



## Trabalhando com vários arquivos

Arquivos individuais são adequados para programas avulsos simples, mas, se você estiver criando um aplicativo mais complexo, provavelmente terá vários arquivos de código no projeto. Vamos nos basear no exemplo de Fibonacci anterior armazenando em cache alguns valores de Fibonacci e adicionar algumas funções recursivas (não se preocupe, recursividade será tratada de forma mais aprofundada mais a frente).

1. Adicione um novo arquivo no diretório *PrimeiroConsole* chamado *FibonacciGenerator.cs* com o seguinte código:

   ```csharp
   using System;
   using System.Collections.Generic;
   
   namespace PrimeiroConsole
   {
       public class FibonacciGenerator
       {
           private Dictionary<int, int> _cache = new Dictionary<int, int>();
           
           private int Fib(int n) => n < 2 ? n : FibValue(n - 1) + FibValue(n - 2);
           
           private int FibValue(int n)
           {
               if (!_cache.ContainsKey(n))
               {
                   _cache.Add(n, Fib(n));
               }
               
               return _cache[n];
           }
           
           public IEnumerable<int> Generate(int n)
           {
               for (int i = 0; i < n; i++)
               {
                   yield return FibValue(i);
               }
           }
       }
   }
   ```

2. Altere o método `Main` no arquivo *Program.cs* para criar uma instância da nova classe e chame seu método como no seguinte exemplo:

   ```csharp
   using System;
   
   namespace PrimeiroConsole
   {
       class Program
       {
           static void Main(string[] args)
           {
               var generator = new FibonacciGenerator();
               foreach (var digit in generator.Generate(15))
               {
                   Console.WriteLine(digit);
               }
           }
       }
   }
   ```

3. Execute [`dotnet build`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-build) para compilar as alterações.

4. Execute o aplicativo executando [`dotnet run`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-run). O seguinte código mostra a saída do programa:

   consoleCopiar

   ```console
   $ dotnet run
   0
   1
   1
   2
   3
   5
   8
   13
   21
   34
   55
   89
   144
   233
   377
   ```

E pronto. Agora, é possível começar a usar os conceitos básicos aprendidos aqui para criar seus próprios programas.

