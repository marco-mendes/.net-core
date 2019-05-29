# C# de teste de unidade no .NET Core usando dotnet test e xUnit

Este laboratório apresenta uma experiência interativa de compilação de uma solução de exemplo passo a passo para aprender os conceitos do teste de unidade. Se você preferir acompanhar o tutorial usando uma solução interna, [veja ou baixe o exemplo de código](https://github.com/dotnet/samples/tree/master/core/getting-started/unit-testing-using-dotnet-test/) antes de começar.

## Criando o projeto de origem

Abra uma janela do terminal. Crie um diretório chamado *unit-testing-using-dotnet-test* para armazenar a solução. Nesse novo diretório, execute [`dotnet new sln`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new) para criar uma nova solução. Ter uma solução facilita o gerenciamento da biblioteca de classes e o projeto de teste de unidade. No diretório da solução, crie um diretório *PrimeService*. A estrutura de arquivo e diretório até aqui deve ser da seguinte forma:

```
/unit-testing-using-dotnet-test
    unit-testing-using-dotnet-test.sln
    /PrimeService
```

Torne *PrimeService* o diretório atual e execute [`dotnet new classlib`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new) para criar o projeto de origem. Renomeie *Class1.cs* como *PrimeService.cs*. Primeiro, crie uma implementação com falha da classe `PrimeService`:

```csharp
using System;

namespace Prime.Services
{
    public class PrimeService
    {
        public bool IsPrime(int candidate)
        {
            throw new NotImplementedException("Please create a test first");
        }
    }
}
```

Altere o diretório de volta para o diretório *unit-testing-using-dotnet-test*.

Execute o comando [dotnet sln](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-sln) para adicionar o projeto de biblioteca de classes à solução:

```
dotnet sln add ./PrimeService/PrimeService.csproj
```

## Criando o projeto de teste

Em seguida, crie o diretório *PrimeService.Tests*. O seguinte esquema mostra a estrutura do diretório:

```
/unit-testing-using-dotnet-test
    unit-testing-using-dotnet-test.sln
    /PrimeService
        Source Files
        PrimeService.csproj
    /PrimeService.Tests
```

Torne o diretório *PrimeService.Tests* o diretório atual e crie um novo projeto usando [`dotnet new xunit`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-new). Esse comando cria um projeto de teste que usa o [xUnit](https://xunit.github.io/) como a biblioteca de teste. O modelo gerado configura o executor de teste no arquivo *PrimeServiceTests.csproj* semelhante ao código a seguir:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.NET.Test.Sdk" Version="15.3.0" />
  <PackageReference Include="xunit" Version="2.2.0" />
  <PackageReference Include="xunit.runner.visualstudio" Version="2.2.0" />
</ItemGroup>
```

O projeto de teste requer outros pacotes para criar e executar testes de unidade. `dotnet new` na etapa anterior adicionou xUnit e o executor de xUnit. Agora, adicione a biblioteca de classes `PrimeService` como outra dependência ao projeto. Use o comando [`dotnet add reference`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-add-reference):

```
dotnet add reference ../PrimeService/PrimeService.csproj
```

Você pode ver o arquivo inteiro no [repositório de exemplos](https://github.com/dotnet/samples/blob/master/core/getting-started/unit-testing-using-dotnet-test/PrimeService.Tests/PrimeService.Tests.csproj) no GitHub.

Veja a seguir o layout da solução final:

```
/unit-testing-using-dotnet-test
    unit-testing-using-dotnet-test.sln
    /PrimeService
        Source Files
        PrimeService.csproj
    /PrimeService.Tests
        Test Source Files
        PrimeServiceTests.csproj
```

Para adicionar o projeto de teste na solução, execute o comando [dotnet sln](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-sln) do diretório *unit-testing-using-dotnet-test*:

```
dotnet sln add ./PrimeService.Tests/PrimeService.Tests.csproj
```

## Criando o primeiro teste

Escreva um teste com falha, faça-o ser aprovado e, em seguida, repita o processo. Remova *UnitTest1.cs* do diretório *PrimeService.Tests* e crie um novo arquivo de C# chamado *PrimeService_IsPrimeShould.cs*. Adicione o seguinte código:

```csharp
using Xunit;
using Prime.Services;

namespace Prime.UnitTests.Services
{
    public class PrimeService_IsPrimeShould
    {
        private readonly PrimeService _primeService;

        public PrimeService_IsPrimeShould()
        {
            _primeService = new PrimeService();
        }

        [Fact]
        public void ReturnFalseGivenValueOf1()
        {
            var result = _primeService.IsPrime(1);

            Assert.False(result, "1 should not be prime");
        }
    }
}
```

O atributo `[Fact]` indica um método de teste que é executado pelo executor de teste. Na pasta *PrimeService.Tests*, execute [`dotnet test`](https://docs.microsoft.com/pt-br/dotnet/core/tools/dotnet-test) para compilar os testes e a biblioteca de classes e execute os testes. O executor de teste do xUnit contém o ponto de entrada do programa para executar os testes. `dotnet test` inicia o executor de teste usando o projeto de teste de unidade que você criou.

O teste falha. Você ainda não criou a implementação. Faça esse teste ser aprovado escrevendo o código mais simples na classe `PrimeService` que funciona. Substitua a implementação de método `IsPrime` pelo código a seguir:

```csharp
public bool IsPrime(int candidate)
{
    if (candidate == 1)
    {
        return false;
    }
    throw new NotImplementedException("Please create a test first");
}
```

No diretório *PrimeService.Tests*, execute `dotnet test` novamente. O comando `dotnet test` executa uma compilação para o projeto `PrimeService` e, depois, para o projeto `PrimeService.Tests`. Depois de compilar os dois projetos, ele executará esse teste único. Ele é aprovado.

## Adicionando mais recursos

Agora que você fez um teste ser aprovado, é hora de escrever mais. Existem alguns outros casos simples de números primos: 0 e -1.Você pode adicionar esses casos como novos testes com o atributo `[Fact]`, mas isso se torna entediante rapidamente. Há outros atributos de xUnit que permitem que você grave um pacote de testes semelhantes:

- O `[Theory]` representa um pacote de testes que executa o mesmo código, mas têm diferentes argumentos de entrada.
- O atributo `[InlineData]` especifica valores para essas entradas.

Em vez de criar novos testes, aplique esses dois atributos, `[Theory]` e `[InlineData]`, para criar uma única teoria no arquivo *PrimeService_IsPrimeShould.cs*. A teoria é um método que testa vários valores inferiores a dois, que é o número primo mais baixo:

```csharp
[Theory]
[InlineData(-1)]
[InlineData(0)]
[InlineData(1)]
public void ReturnFalseGivenValuesLessThan2(int value)
{
    var result = _primeService.IsPrime(value);
    
    Assert.False(result, $"{value} should not be prime");
}
```

Execute `dotnet test` novamente, e dois desses testes deverão falhar. Para fazer com que todos os testes sejam aprovados, altere a cláusula `if` no início do método `IsPrime` no arquivo *PrimeService.cs*:

```csharp
if (candidate < 2)
```



## Testando com Docker

Para que possamos testar durante o processo de build da imagem e utilizar a bibliteca durante o runtime, primeiro teremos que criar um novo projeto na solução que faz uso da biblioteca PrimeService.

Para isso, vamos criar uma pasta nova chamada Terminal, navegar até dela e criar um novo projeto com o comando `dotnet new console`

```
mkdir Terminal
cd Terminal
dotnet new console
```

Agora temos que adicionar a referência ao pacote do PrimeService. Para isso, vamos subir um nível de pasta e referenciar cada projeto através do path relativo.

```
cd ..
dotnet add Terminal/Terminal.csproj reference PrimeService/PrimeService.csproj
```

Dessa forma, estaremos especificando que o projeto do Terminal faz referência ao projeto do PrimeService através da linha de comando.

Alteramos então o código de Terminal/Program.cs para o seguinte:

````c#
using System;
using Prime.Services;

namespace Terminal
{
    public class Program {

        static void Main(string[] args)
        {
            var _primeService = new PrimeService();
            var counter = 0;
            var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
            while(max == -1 || counter < max)
            {
                counter++;
                Console.WriteLine($"Counter: {counter}");
                Console.WriteLine($"Is Prime: {_primeService.IsPrime(counter)}");
                System.Threading.Tasks.Task.Delay(1000).Wait();
            }
        }
    }
}
````

Assim, estaremos utilizando a biblioteca anteriormente criada e o código do Laboratório de Docker para criarmos um container contendo toda nossa solução.

No processo de build temos que incluir todo o conteúdo da solução, realizar o restore, build e publicação da nossa aplicação chamada Terminal para um container de runtime. O Dockerfile então ficaria parecido com o abaixo:

```dockerfile
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src

# Copiar solução e restaurar dependencias
COPY . ./
RUN dotnet restore
# Realizando testes de unidade em PrimeService
RUN dotnet test
# Fazendo build completa
RUN dotnet build
# Publicando aplicação console em pasta Terminal/out
RUN dotnet publish "Terminal/Terminal.csproj" -c Release -o out

# Criação da imagem final
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /src/Terminal/out .
ENTRYPOINT ["dotnet", "Terminal.dll"]
```



Agora é necessário realizar a build da imagem, criação de um container e execução do mesmo, através dos comandos:

```bash
docker build -t xunit .
```

A saída do comando de build deve ser parecida com:

```bash
Step 1/11 : FROM microsoft/dotnet:2.2-sdk AS build-env
 ---> e4747ec2aaff
Step 2/11 : WORKDIR /src
 ---> Using cache
 ---> 5114d0b5421a
Step 3/11 : COPY . ./
 ---> 462a9aae988b
Step 4/11 : RUN dotnet restore
 ---> Running in a2ced24a20c5
  Restore completed in 199.32 ms for /src/PrimeService/PrimeService.csproj.
  Restore completed in 6.13 sec for /src/PrimeService.Tests/PrimeService.Tests.csproj.
Removing intermediate container a2ced24a20c5
 ---> a482ffdd597b
Step 5/11 : RUN dotnet test
 ---> Running in 789a50d7c506
Test run for /src/PrimeService.Tests/bin/Debug/netcoreapp2.2/PrimeService.Tests.dll(.NETCoreApp,Version=v2.2)
Microsoft (R) Test Execution Command Line Tool Version 16.1.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Test Run Successful.
Total tests: 11
     Passed: 11
 Total time: 1.1042 Seconds
Removing intermediate container 789a50d7c506
 ---> a9748b592481
Step 6/11 : RUN dotnet build
 ---> Running in af2728cf731f
Microsoft (R) Build Engine version 16.1.76+g14b0a930a7 for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 32.55 ms for /src/PrimeService/PrimeService.csproj.
  Restore completed in 33.11 ms for /src/PrimeService.Tests/PrimeService.Tests.csproj.
  PrimeService -> /src/PrimeService/bin/Debug/netstandard2.0/PrimeService.dll
  PrimeService.Tests -> /src/PrimeService.Tests/bin/Debug/netcoreapp2.2/PrimeService.Tests.dll

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:02.42
Removing intermediate container af2728cf731f
 ---> 5291d7e64036
Step 7/11 : RUN dotnet publish "Terminal/Terminal.csproj" -c Release -o out
 ---> Running in 3ec8d9d24e77
Microsoft (R) Build Engine version 16.1.76+g14b0a930a7 for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 36.35 ms for /src/PrimeService/PrimeService.csproj.
  Restore completed in 131.2 ms for /src/Terminal/Terminal.csproj.
  PrimeService -> /src/PrimeService/bin/Release/netstandard2.0/PrimeService.dll
  Terminal -> /src/Terminal/bin/Release/netcoreapp2.2/Terminal.dll
  Terminal -> /src/Terminal/out/
Removing intermediate container 3ec8d9d24e77
 ---> 8b29d842f5e5
Step 8/11 : FROM microsoft/dotnet:2.2-aspnetcore-runtime
 ---> f6d51449c477
Step 9/11 : WORKDIR /app
 ---> Running in 36f3102790fb
Removing intermediate container 36f3102790fb
 ---> a79b8cc00e51
Step 10/11 : COPY --from=build-env /src/Terminal/out .
 ---> c97e04d29846
Step 11/11 : ENTRYPOINT ["dotnet", "Terminal.dll"]
 ---> Running in fd8cf56dbd4d
Removing intermediate container fd8cf56dbd4d
 ---> 77db48d64d76
Successfully built 77db48d64d76
Successfully tagged xunit:latest
```

> Note que todos os 11 testes foram executados com sucesso.

E agora os comandos para criar um container e iniciá-lo:

```bash
docker create xunit
docker start <ID ou Nome do Container Criado>
```

Para encontrar o ID ou Nome do container criado, como visto anteriormente, é necessário utilizar o comando `docker ps -a`.

Com o container em execução, é possível inspecionar o que está acontecendo internamente com o comando `docker attach --sig-proxy=false <ID ou Nome do Container Criado>`. A saída deve ser parecida com:

```
Counter: 13
Is Prime: True
Counter: 14
Is Prime: False
Counter: 15
Is Prime: False
Counter: 16
Is Prime: False
Counter: 17
Is Prime: True
Counter: 18
Is Prime: False
Counter: 19
Is Prime: True
Counter: 20
Is Prime: False
Counter: 21
IS Prime: False
```

Assim, temos um Dockerfile que faz a build completa do serviço incluindo testes sobre a biblioteca principal.



