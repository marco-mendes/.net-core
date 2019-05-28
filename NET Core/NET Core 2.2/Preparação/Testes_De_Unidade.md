# Teste de unidade no .NET Core e no .NET Standard

O .NET Core torna mais fácil criar testes de unidade. Este artigo apresenta brevemente os testes de unidade e mostra como eles diferem de outros tipos de testes. Os recursos vinculados na parte inferior da página mostram como adicionar um projeto de teste à sua solução. Depois de configurar seu projeto de teste, você poderá executar testes de unidade usando a linha de comando ou o Visual Studio.

O .NET Core 2.0 e posterior dá suporte ao [.NET Standard 2.0](https://docs.microsoft.com/pt-br/dotnet/standard/net-standard). Usamos as bibliotecas dele para demonstrar os testes de unidades.

Você é capaz de usar modelos de projeto de teste de unidade internos do .NET Core 2.0 e posteriores para C#, F# e Visual Basic como um ponto de partida para seu projeto pessoal.

## O que são testes de unidade?

Testes automatizados são uma ótima maneira de garantir que um aplicativo de software faça o que seus autores pretendem. Existem vários tipos de testes para aplicativos de software. Isso inclui testes de integração, testes na Web, testes de carga e outros. **Testes de unidade** testam métodos e componentes de software individuais. Testes de unidade devem testar apenas o código dentro do controle do desenvolvedor. Eles não devem testar questões de infraestrutura. Questões de infraestrutura incluem bancos de dados, sistemas de arquivos e recursos de rede.

Além disso, tenha em mente que há práticas recomendadas para escrever testes. Por exemplo, um [TDD (Desenvolvimento Orientado por Testes)](https://deviq.com/test-driven-development/) é quando um teste de unidade é escrito antes do código que ele é destinado a verificar. O TDD é como criar um sumário para um livro antes de escrevê-lo. Ele destina-se a ajudar os desenvolvedores a escrever código mais legível, simples e eficiente.

 **Observação**

A equipe do ASP.NET segue [estas convenções](https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests) para ajudar os desenvolvedores a inventar bons nomes para os métodos e classes de teste.

Tente não introduzir dependências na infraestrutura ao escrever testes de unidade. Elas tornam os testes lentos e frágeis, devendo ser reservadas para testes de integração. Você pode evitar essas dependências no aplicativo seguindo o [Princípio de Dependências Explícitas](https://deviq.com/explicit-dependencies-principle/) e usando a [Injeção de Dependência](https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/dependency-injection). Você também pode manter seus testes de unidade em um projeto separado de seus testes de integração. Isso garante que seu projeto de teste de unidade não tenha dependências de pacotes de infraestrutura nem referências a eles.



# Melhores práticas de teste de unidade com .NET Core e .NET Standard



Há diversas vantagens para escrever testes de unidade; eles ajudam com a regressão, fornecem a documentação e facilitam o bom design. No entanto, testes de unidade difíceis de ler e frágeis podem causar estragos na sua base de código. Este artigo descreve algumas melhores práticas com relação ao design de teste de unidade para projetos .NET Core e .NET Standard.

Neste guia, você aprenderá algumas melhores práticas ao escrever testes de unidade para manter seus testes resilientes e fáceis de entender.

> Por [John Reese](https://reese.dev/) com agradecimentos especiais a [Roy Osherove](https://osherove.com/)

## Por que o teste de unidade?

### Menos tempo realizando testes funcionais

Testes funcionais são caros. Normalmente, envolvem abrir o aplicativo e realizar uma série de etapas que você (ou outra pessoa) deve seguir a fim de validar o comportamento esperado. Essas etapas nem sempre podem ser de conhecimento do testador, o que significa que ele terá que entrar em contato com alguém com maior conhecimento na área para executar o teste. O teste em si pode levar segundos para alterações triviais ou minutos para alterações maiores. Por fim, esse processo deve ser repetido para todas as alterações feitas no sistema.

Testes de unidade, por outro lado, demoram milissegundos, podem ser executados ao pressionar um botão e não exigem, necessariamente, nenhum conhecimento do sistema em geral. A aprovação ou reprovação no teste cabe ao executor do teste, não ao indivíduo.

### Proteção contra regressão

Defeitos de regressão são defeitos introduzidos quando uma alteração foi feita no aplicativo. É comum os testadores não só testarem seu novo recurso, mas também recursos que existiam antes a fim de verificar se recursos previamente implementados ainda funcionam conforme o esperado.

Com o teste de unidade, é possível executar novamente todo o conjunto de testes após todo build ou até mesmo após a alteração de uma linha de código. Você poderá confiar que o novo código não interromperá a funcionalidade existente.

### Documentação executável

Nem sempre pode ser óbvio o que um método específico faz ou como ele se comporta diante de uma determinada entrada. Você pode se perguntar: Como esse método se comportará se eu passar uma cadeia de caracteres em branco? Nulo?

Quando você tem um conjunto de testes de unidade bem nomeados, cada teste deve ser capaz de explicar claramente a saída esperada de uma determinada entrada. Além disso, ele deve ser capaz de verificar se isso realmente funciona.

### Código menos acoplado

Pode ser difícil para o teste de unidade quando o código está bem acoplado. Sem criar testes de unidade para o código que você está escrevendo, o acoplamento pode ser menos aparente.

Escrever testes para seu código o desacoplará naturalmente, porque seria mais difícil testar, caso contrário.

## Características de um bom teste de unidade

- **Rápido**. Não é incomum para projetos maduros ter milhares de testes de unidade. Os testes de unidade devem levar muito pouco tempo para serem executados. Milissegundos.
- **Isolado**. Testes de unidade são autônomos, podem ser executados em isolamento e não têm dependências em nenhum fator externo, como um sistema de arquivos ou o banco de dados.
- **Repetível**. A execução de um teste de unidade deve ser consistente com seus resultados, ou seja, ele sempre retornará o mesmo resultado se você não alterar nada entre execuções.
- **Verificação automática**. O teste deve ser capaz de detectar automaticamente se ele foi aprovado ou reprovado sem nenhuma interação humana.
- **Em tempo hábil**. Um teste de unidade não deve levar um tempo desproporcionalmente longo para ser escrito comparado com o código que está sendo testado. Se você achar que o teste do código está levando uma grande quantidade de tempo comparado com a escrita do código, considere um design mais testável.

## Vamos falar a mesma língua

O termo *simular* infelizmente é usado incorretamente quando falamos sobre o teste. Os itens a seguir definem os tipos mais comuns de *falsificações* ao escrever testes de unidade:

*Falsificação* – Uma falsificação é um termo genérico que pode ser usado para descrever um stub ou um objeto fictício. Ser um stub ou uma simulação depende do contexto no qual ele é usado. Então, em outras palavras, uma falsificação pode ser um stub ou uma simulação.

*Simulação* -um objeto fictício é um objeto falso no sistema que decide se um teste de unidade foi aprovado ou não. Uma simulação começa como falsificação até ser incorrida.

*Stub* – Um stub é uma substituição controlável para uma dependência existente (ou colaborador) no sistema. Ao usar um stub, é possível testar seu código sem lidar diretamente com a dependência. Por padrão, uma falsificação começa como um stub.

Considere o snippet de código a seguir:

```csharp
var mockOrder = new MockOrder();
var purchase = new Purchase(mockOrder);

purchase.ValidateOrders();

Assert.True(purchase.CanBeShipped);
```

Este seria um exemplo de um stub sendo chamado de simulação. Nesse caso, ele é um stub. Você está passando a ordem como um meio de poder instanciar `Purchase` (o sistema em teste). O nome `MockOrder` também é muito enganoso, porque novamente, a ordem não é uma simulação.

Uma abordagem melhor seria

```csharp
var stubOrder = new FakeOrder();
var purchase = new Purchase(stubOrder);

purchase.ValidateOrders();

Assert.True(purchase.CanBeShipped);
```

Renomeando a classe para `FakeOrder`, você tornou a classe muito mais genérica, a classe pode ser usada como uma simulação ou um stub. O que for melhor para o caso de teste. No exemplo acima, `FakeOrder` é usado como um stub. Você não está usando o `FakeOrder` em qualquer forma durante a declaração. `FakeOrder` apenas foi passado para a classe `Purchase` para satisfazer os requisitos do construtor.

Para usá-lo como uma Simulação, você poderia fazer algo como isto

```csharp
var mockOrder = new FakeOrder();
var purchase = new Purchase(mockOrder);

purchase.ValidateOrders();

Assert.True(mockOrder.Validated);
```

Nesse caso, você está verificando uma propriedade na Falsificação (declarando em relação a ela), então no snippet de código acima, o `mockOrder` é uma Simulação.

 **Importante**

É importante usar esta terminologia corretamente. Se você chamar seus stubs de "simulações", outros desenvolvedores farão suposições falsas sobre sua intenção.

O ponto principal a lembrar sobre simulações versus stub é que simulações são como stubs, mas você declara com relação ao objeto fictício, enquanto você não declara com relação a um stub.

## Práticas recomendadas

### Nomeando seus testes

O nome do seu teste deve ser composto por três partes:

- O nome do método que está sendo testado.
- O cenário em que ele está sendo testado.
- O comportamento esperado quando o cenário é invocado.

#### Por quê?

- Padrões de nomenclatura são importantes, porque eles expressam explicitamente a intenção do teste.

Testes são mais do que apenas verificar se seu código funciona; eles também fornecem documentação. Apenas examinando o conjunto de testes de unidade, você deve poder inferir o comportamento do seu código sem mesmo examinar o código em si. Além disso, quando os testes falharem, será possível ver exatamente quais cenários não atendem às suas expectativas.

#### Ruim:

```csharp
[Fact]
public void Test_Single()
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add("0");

    Assert.Equal(0, actual);
}
```

#### Melhor:

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();
    
    var actual = stringCalculator.Add("0");
    
    Assert.Equal(0, actual);
}
```

### Organizando seus testes

**Organizar, Agir, Declarar** é um padrão comum ao testar unidades. Como o nome implica, ele é composto por três ações principais:

- *Organizar* seus objetos, criando e configurando-os conforme necessário.
- *Agir* sobre um objeto.
- *Declarar* que algo está conforme o esperado.

#### Por quê?

- Isso separa claramente o que está sendo testado das etapas *organizar* e *declarar*.
- Menos oportunidade de combinar declarações com o código "Agir".

A legibilidade é um dos aspectos mais importante ao escrever um teste. A separação de cada uma dessas ações dentro do teste realça claramente as dependências necessárias para chamar seu código, a maneira como seu código está sendo chamado e o que você está tentando declarar. Embora seja possível combinar algumas etapas e reduzir o tamanho do seu teste, a principal meta é tornar o teste o mais legível possível.

#### Ruim:

```csharp
[Fact]
public void Add_EmptyString_ReturnsZero()
{
    // Arrange
    var stringCalculator = new StringCalculator();

    // Assert
    Assert.Equal(0, stringCalculator.Add(""));
}
```

#### Melhor:

```csharp
[Fact]
public void Add_EmptyString_ReturnsZero()
{
    // Arrange
    var stringCalculator = new StringCalculator();
    
    // Act         
    var actual = stringCalculator.Add("");

    // Assert
    Assert.Equal(0, actual);
}
```

### Escrever testes com o mínimo de aprovação

A entrada a ser usada em um teste de unidade deve ser a mais simples possível a fim de verificar o comportamento que você está testando no momento.

#### Por quê?

- Os testes se tornam mais resilientes a alterações futuras na base de código.
- Mais próximo do comportamento do teste ao longo da implementação.

Testes que incluem mais informações do que as necessárias para serem aprovados têm maior chance de introduzir erros no teste e podem deixar a intenção dele menos clara. Ao escrever testes, concentre-se no comportamento. Definir propriedades extra em modelos ou usar valores diferentes de zero quando não for necessário só fará desviar do que você está tentando provar.

#### Ruim:

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add("42");

    Assert.Equal(42, actual);
}
```

#### Melhor:

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();
    
    var actual = stringCalculator.Add("0");
    
    Assert.Equal(0, actual);
}
```

### Evitar cadeias de caracteres mágicas

Nomear variáveis em testes de unidade é tão importante, se não mais importante, do que nomear variáveis no código de produção.Os testes de unidade não devem conter cadeias de caracteres mágicas.

#### Por quê?

- Evita a necessidade de o leitor do teste inspecionar o código de produção a fim de descobrir o que torna o valor especial.
- Mostra explicitamente o que você está tentando *provar*, em vez de o que você está tentando *realizar*.

Cadeias de caracteres mágicas podem gerar confusão no leitor dos seus testes. Se uma cadeia de caracteres parecer estar fora do comum, eles poderão ser perguntar por que um determinado valor foi escolhido para um parâmetro ou valor retornado. Talvez isso os leve a examinar mais detalhadamente os detalhes da implementação, em vez de se concentrar no teste.

 Dica

Ao escrever testes, seu objetivo deve ser expressar o máximo de intenção possível. No caso de cadeias de caracteres mágicas, uma boa abordagem é atribuir esses valores a constantes.

#### Ruim:

```csharp
[Fact]
public void Add_BigNumber_ThrowsException()
{
    var stringCalculator = new StringCalculator();

    Action actual = () => stringCalculator.Add("1001");

    Assert.Throws<OverflowException>(actual);
}
```

#### Melhor:

```csharp
       [Fact]
       void Add_MaximumSumResult_ThrowsOverflowException()
       {
           var stringCalculator = new StringCalculator();
           const string MAXIMUM_RESULT = "1001";

           Action actual = () => stringCalculator.Add(MAXIMUM_RESULT);

           Assert.Throws<OverflowException>(actual);
       }
```

### Evitar a lógica em testes

Ao escrever seus testes de unidade, evite a concatenação manual de cadeia de caracteres e condições lógicas como `if`, `while`, `for`, `switch`, etc.

#### Por quê?

- Menor chance de introduzir um bug dentro dos seus testes.
- Concentre-se no resultado final, em vez de nos detalhes da implementação.

Quando você introduz a lógica em seu conjunto de testes, a possibilidade de introduzir um bug nela aumenta drasticamente. O último lugar que você deseja encontrar um bug é dentro do seu conjunto de testes. Você deve ter um alto nível de confiança de que seus testes funcionam; caso contrário, você não confiará neles. Testes nos quais você não confia não fornecem nenhum valor.Quando um teste falha, convém ter uma noção de que algo está realmente errado com o seu código e de que isso não pode ser ignorado.

 Dica

Se a lógica em seu teste parecer inevitável, considere dividir o teste em dois ou mais testes diferentes.

#### Ruim:

```csharp
[Fact]
public void Add_MultipleNumbers_ReturnsCorrectResults()
{
    var stringCalculator = new StringCalculator();
    var expected = 0;
    var testCases = new[]
    {
        "0,0,0",
        "0,1,2",
        "1,2,3"
    };

    foreach (var test in testCases)
    {
        Assert.Equal(expected, stringCalculator.Add(test));
        expected += 3;
    }

}
```

#### Melhor:

```csharp
[Theory]
[InlineData("0,0,0", 0)]
[InlineData("0,1,2", 3)]
[InlineData("1,2,3", 6)]
public void Add_MultipleNumbers_ReturnsSumOfNumbers(string input, int expected)
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add(input);

    Assert.Equal(expected, actual);
}
```

### Preferir métodos auxiliares para instalação (setup) e desinstalação (teardown)

Se você precisar de um objeto ou estado semelhante para seus testes, prefira um método auxiliar do que usar atributos Setup e Teardown, se existirem.

#### Por quê?

- Menos confusão ao ler os testes, uma vez que todo o código está visível de dentro de cada teste.
- Menos chance de configurar demais ou de menos para o teste específico.
- Menos chance de compartilhar estado entre testes, o que cria dependências indesejadas entre eles.

Nas estruturas de teste de unidade, `Setup` é chamado antes de cada teste de unidade dentro do seu conjunto de testes. Embora alguns possam conceber isso como uma ferramenta útil, ela geralmente acaba levando a testes difíceis de ler e sobrecarregados.Cada teste geralmente terá requisitos diferentes para fazer o teste entrar em funcionamento. Infelizmente, `Setup` força você a usar os mesmos requisitos exatos para cada teste.

 Observação

O xUnit removeu SetUp e TearDown a partir da versão 2.x

#### Ruim:

```csharp
private readonly StringCalculator stringCalculator;
public StringCalculatorTests()
{
    stringCalculator = new StringCalculator();
}
```

```csharp
// more tests...
```

```csharp
[Fact]
public void Add_TwoNumbers_ReturnsSumOfNumbers()
{
    var result = stringCalculator.Add("0,1");

    Assert.Equal(1, result);
}
```

#### Melhor:

```csharp
[Fact]
public void Add_TwoNumbers_ReturnsSumOfNumbers()
{
    var stringCalculator = CreateDefaultStringCalculator();

    var actual = stringCalculator.Add("0,1");

    Assert.Equal(1, actual);
}
```

```csharp
// more tests...
```

```csharp
private StringCalculator CreateDefaultStringCalculator()
{
    return new StringCalculator();
}
```

### Evitar várias declarações

Ao escrever seus testes, tente incluir apenas uma declaração por teste. Abordagens comuns para usar apenas uma declaração incluem:

- Criar um teste separado para cada declaração.
- Usar testes parametrizados.

#### Por quê?

- Se uma declaração falhar, as subsequentes não serão avaliadas.
- Garante que você não está declarando vários casos em seus testes.
- Oferece a você todo o panorama do porquê seus testes estão falhando.

Ao introduzir várias declarações em um caso de teste, não há garantida de que todas elas serão executadas. Na maioria das estruturas de teste de unidade, após uma declaração falhar em um teste de unidade, os testes seguintes serão considerados reprovados automaticamente. Isso pode ser confuso, porque a funcionalidade que está realmente em funcionamento será mostrada como falha.

 Observação

Uma exceção comum a essa regra é ao declarar em relação a um objeto. Nesse caso, é geralmente aceitável ter várias declarações com relação a cada propriedade para garantir que o objeto está no estado em que você espera que ele esteja.

#### Ruim:

```csharp
[Fact]
public void Add_EdgeCases_ThrowsArgumentExceptions()
{
    Assert.Throws<ArgumentException>(() => stringCalculator.Add(null));
    Assert.Throws<ArgumentException>(() => stringCalculator.Add("a"));
}
```

#### Melhor:

```csharp
    [Theory]
    [InlineData(null)]
    [InlineData("a")]
    public void Add_InputNullOrAlphabetic_ThrowsArgumentException(string input)
    {
        var stringCalculator = new StringCalculator();

        Action actual = () => stringCalculator.Add(input);

        Assert.Throws<ArgumentException>(actual);
    }
```

### Validar métodos privados por métodos públicos de teste de unidade

Na maioria dos casos, não deve haver a necessidade de testar um método privado. Métodos privados são um detalhe de implementação. Você pode pensar dessa forma: métodos privados nunca existem isoladamente. Em algum momento, haverá um método voltado para o público que chame o método privado como parte de sua implementação. O que importa é o resultado final do método público que é chamado no privado.

Considere o seguinte caso

```csharp
public string ParseLogLine(string input)
{
    var sanitizedInput = TrimInput(input);
    return sanitizedInput;
}

private string TrimInput(string input)
{
    return input.Trim();
}
```

Sua primeira reação pode ser começar a escrever um teste para `TrimInput` porque você deseja certificar-se de que o método está funcionando conforme o esperado. No entanto, é totalmente possível que `ParseLogLine` manipule `sanitizedInput` de uma maneira inesperada, tornando um teste em relação a `TrimInput` inútil.

O teste real deve ser feito em relação ao método voltado para o público `ParseLogLine`, porque esse é o que importa no fim das contas.

```csharp
public void ParseLogLine_ByDefault_ReturnsTrimmedResult()
{
    var parser = new Parser();

    var result = parser.ParseLogLine(" a ");

    Assert.Equals("a", result);
}
```

Com esse ponto de vista, se você vir um método privado, encontre o método público e escreva seus testes com relação a esse método. Só porque um método privado retorna o resultado esperado não significa que o sistema que eventualmente chama o método privado usa o resultado corretamente.

### Referências estáticas de stub

Um dos princípios de um teste de unidade é que ele deve ter controle completo do sistema em teste. Isso pode ser problemático quando o código de produção inclui chamadas às referências estáticas (por exemplo, `DateTime.Now`). Considere o código a seguir

```csharp
public int GetDiscountedPrice(int price)
{
    if(DateTime.Now == DayOfWeek.Tuesday) 
    {
        return price / 2;
    }
    else 
    {
        return price;
    }
}
```

Como esse código pode possivelmente ter unidades testadas? Você pode tentar uma abordagem como

```csharp
public void GetDiscountedPrice_ByDefault_ReturnsFullPrice()
{
    var priceCalculator = new PriceCalculator();

    var actual = priceCalculator.GetDiscountedPrice(2);

    Assert.Equals(2, actual)
}

public void GetDiscountedPrice_OnTuesday_ReturnsHalfPrice()
{
    var priceCalculator = new PriceCalculator();

    var actual = priceCalculator.GetDiscountedPrice(2);

    Assert.Equals(1, actual);
}
```

Infelizmente, você perceberá rapidamente que há alguns problemas com seus testes.

- Se o conjunto de testes for executado em uma terça-feira, o segundo teste será aprovado, mas o primeiro não.
- Se o conjunto de testes for executado em qualquer outro dia, o primeiro teste será aprovado, mas o segundo não.

Para resolver esses problemas, será necessário introduzir uma *costura* em seu código de produção. Uma abordagem é encapsular o código que você precisa controlar em uma interface e fazer o código de produção depender dessa interface.

```csharp
public interface IDateTimeProvider
{
    DayOfWeek DayOfWeek();
}

public int GetDiscountedPrice(int price, IDateTimeProvider dateTimeProvider)
{
    if(dateTimeProvider.DayOfWeek() == DayOfWeek.Tuesday) 
    {
        return price / 2;
    }
    else 
    {
        return price;
    }
}
```

Seu conjunto de testes agora se torna

```csharp
public void GetDiscountedPrice_ByDefault_ReturnsFullPrice()
{
    var priceCalculator = new PriceCalculator();
    var dateTimeProviderStub = new Mock<IDateTimeProvider>();
    dateTimeProviderStub.Setup(dtp => dtp.DayOfWeek()).Returns(DayOfWeek.Monday);

    var actual = priceCalculator.GetDiscountedPrice(2, dateTimeProviderStub);

    Assert.Equals(2, actual);
}

public void GetDiscountedPrice_OnTuesday_ReturnsHalfPrice()
{
    var priceCalculator = new PriceCalculator();
    var dateTimeProviderStub = new Mock<IDateTimeProvider>();
    dateTimeProviderStub.Setup(dtp => dtp.DayOfWeek()).Returns(DayOfWeek.Tuesday);

    var actual = priceCalculator.GetDiscountedPrice(2, dateTimeProviderStub);

    Assert.Equals(1, actual);
}
```

Agora o conjunto de testes tem controle total sobre `DateTime.Now` e pode fazer stub de qualquer valor ao ser chamado no método.