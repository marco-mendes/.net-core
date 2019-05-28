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