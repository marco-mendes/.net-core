# O modelo de programação assíncrono Task em C#

O modelo TAP (programação assíncrona Task) proporciona uma abstração em código assíncrono. Você escreve o código como uma sequência de instruções, como usual. Você pode ler o código como se cada instrução fosse concluída antes do início da próxima. O compilador realiza uma série de transformações, porque algumas dessas instruções podem iniciar o trabalho e retornar um [Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task) que representa o trabalho em andamento.

Essa é a meta dessa sintaxe: habilitar um código que leia como uma sequência de instruções, mas que execute em uma ordem muito mais complicada com base na alocação de recurso externo e em quando as tarefas são concluídas. Isso é semelhante à maneira como as pessoas dão instruções para processos que incluem tarefas assíncronas. Neste artigo, você usará um exemplo com instruções para fazer um café da manhã e ver como as palavras-chave `async` e `await` facilitam raciocinar sobre o código que inclui uma série de instruções assíncronas. Você deve escrever as instruções de maneira parecida com a lista a seguir para explicar como fazer um café da manhã:

1. Encher uma xícara de café.
2. Aquecer uma frigideira e, em seguida, fritar dois ovos.
3. Frita três fatias de bacon.
4. Torrar dois pedaços de pão.
5. Adicionar manteiga e a geleia na torrada.
6. Encher um copo com suco de laranja.

Se tivesse experiência em culinária, você executaria essas instruções **assincronamente**. Você iniciaria aquecendo a frigideira para os ovos e, em seguida, começaria a preparar o bacon. Você colocaria o pão na torradeira e começaria a preparar os ovos. Em cada etapa do processo, iniciaria uma tarefa e voltaria sua atenção para as tarefas que estivessem prontas para a sua atenção.

Preparar o café da manhã é um bom exemplo de trabalho assíncrono que não é paralelo. Uma pessoa (ou um thread) pode lidar com todas essas tarefas. Continuando com a analogia do café da manhã, uma pessoa pode fazer café da manhã assincronamente iniciando a tarefa seguinte antes de concluir a primeira. O preparo progride independentemente de haver alguém observando. Assim que inicia o aquecimento da frigideira para os ovos, você pode começar a fritar o bacon. Quando começar a preparar o bacon, você pode colocar o pão na torradeira.

Para um algoritmo paralelo, você precisaria de vários cozinheiros (ou threads). Um prepararia os ovos, outro o bacon e assim por diante. Cada um se concentraria apenas naquela tarefa específica. Cada cozinheiro (ou thread) ficaria bloqueado de forma síncrona, esperando que o bacon estivesse pronto para ser virado ou que a torrada pulasse.

Agora, considere essas mesmas instruções escritas como instruções em C#:



```csharp
static void Main(string[] args)
{
    Coffee cup = PourCoffee();
    Console.WriteLine("coffee is ready");
    Egg eggs = FryEggs(2);
    Console.WriteLine("eggs are ready");
    Bacon bacon = FryBacon(3);
    Console.WriteLine("bacon is ready");
    Toast toast = ToastBread(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("toast is ready");
    Juice oj = PourOJ();
    Console.WriteLine("oj is ready");

    Console.WriteLine("Breakfast is ready!");
}
```

Os computadores não interpretam essas instruções da mesma forma que as pessoas. O computador ficará bloqueado em cada instrução até que o trabalho seja concluído, antes de passar para a próxima instrução. Isso cria um café da manhã insatisfatório. As tarefas posteriores não seriam iniciadas até que as tarefas anteriores fossem concluídas. Levaria muito mais tempo para criar o café da manhã e alguns itens ficariam frios antes de serem servidos.

Se você quiser que o computador execute as instruções acima de forma assíncrona, deverá escrever o código assíncrono.

Essas questões são importantes para os programas que você escreve atualmente. Ao escrever programas de cliente, você quer que a interface do usuário responda de acordo com as solicitações do usuário. Seu aplicativo não deve fazer um telefone parecer travado enquanto ele está baixando dados da Web. Ao escrever programas de servidor, você não quer threads bloqueados. Esses threads poderiam servir a outras solicitações. O uso de código síncrono quando existem alternativas assíncronas afeta sua capacidade de aumentar de forma menos custosa. Você paga pelos threads bloqueados.

Aplicativos modernos bem-sucedidos exigem código assíncrono. Sem suporte de linguagem, escrever código assíncrono exigia retornos de chamada, eventos de conclusão ou outros meios que obscureciam a intenção original do código. A vantagem do código síncrono é que ele é fácil de entender. As ações passo a passo facilitam o exame e o entendimento. Modelos assíncronos tradicionais forçavam você a se concentrar na natureza assíncrona do código e não nas ações fundamentais do código.

## Não bloquear, mas aguardar

O código anterior demonstra uma prática inadequada: construção de código síncrono para realizar operações assíncronas. Como escrito, esse código bloqueia o thread que o está executando, impedindo-o de realizar qualquer outra tarefa. Ele não será interrompido enquanto qualquer uma das tarefas estiver em andamento. Seria como se você fixasse o olhar na torradeira depois de colocar o pão. Você ignoraria qualquer pessoa que estivesse conversando com você até que a torrada pulasse.

Vamos começar atualizando esse código para que o thread não seja bloqueado enquanto houver tarefas em execução. A palavra-chave `await` oferece uma maneira sem bloqueio de iniciar uma tarefa e, em seguida, continuar a execução quando essa tarefa for concluída. Uma versão assíncrona simples do código de fazer café da manhã ficaria como o snippet a seguir:



```csharp
static async Task Main(string[] args)
{
    Coffee cup = PourCoffee();
    Console.WriteLine("coffee is ready");
    Egg eggs = await FryEggs(2);
    Console.WriteLine("eggs are ready");
    Bacon bacon = await FryBacon(3);
    Console.WriteLine("bacon is ready");
    Toast toast = await ToastBread(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("toast is ready");
    Juice oj = PourOJ();
    Console.WriteLine("oj is ready");

    Console.WriteLine("Breakfast is ready!");
}
```

Esse código não bloqueia enquanto os ovos ou o bacon são preparados. Entretanto, esse código não iniciará outras tarefas. Você ainda colocaria o pão na torradeira e ficaria olhando até ele pular. Mas, pelo menos, você responderia a qualquer pessoa que quisesse sua atenção. Em um restaurante em que vários pedidos são feitos, o cozinheiro pode iniciar o preparo de outro café da manhã enquanto prepara o primeiro.

Agora, o thread trabalhando no café da manhã não fica bloqueado aguardando qualquer tarefa iniciada que ainda não tenha terminado. Para alguns aplicativos, essa alteração já basta. Um aplicativo de GUI ainda responde ao usuário com apenas essa alteração. No entanto, neste cenário, você quer mais. Você não deseja que cada uma das tarefas componentes seja executada em sequência. É melhor iniciar cada uma das tarefas componentes antes de aguardar a conclusão da tarefa anterior.

## Iniciar tarefas simultaneamente

Em muitos cenários, convém iniciar várias tarefas independentes imediatamente. Em seguida, conforme cada tarefa é concluída, você pode continuar outro trabalho que esteja pronto. Na analogia do café da manhã, é assim que você prepara o café da manhã muito mais rapidamente. Você também prepara tudo quase ao mesmo tempo. Você terá um café da manhã quente.

O [System.Threading.Tasks.Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task) e os tipos relacionados são classes que você pode usar para pensar nas tarefas que estão em andamento. Elas permitem que você escreva código que se assemelhe mais à maneira como você realmente prepara o café da manhã. Você começaria a preparar os ovos, o bacon e a torrada ao mesmo tempo. Como cada um exige ação, você voltaria sua atenção para essa tarefa, cuidaria da próxima ação e aguardaria algo mais que exigisse sua atenção.

Você inicia uma tarefa e espera o objeto [Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task) que representa o trabalho. Você vai `await` cada tarefa antes de trabalhar com o respectivo resultado.

Vamos fazer essas alterações no código do café da manhã. A primeira etapa é armazenar as tarefas para as operações quando elas forem iniciadas, em vez de aguardá-las:



```csharp
Coffee cup = PourCoffee();
Console.WriteLine("coffee is ready");
Task<Egg> eggTask = FryEggs(2);
Egg eggs = await eggTask;
Console.WriteLine("eggs are ready");
Task<Bacon> baconTask = FryBacon(3);
Bacon bacon = await baconTask;
Console.WriteLine("bacon is ready");
Task<Toast> toastTask = ToastBread(2);
Toast toast = await toastTask;
ApplyButter(toast);
ApplyJam(toast);
Console.WriteLine("toast is ready");
Juice oj = PourOJ();
Console.WriteLine("oj is ready");

Console.WriteLine("Breakfast is ready!");
```

Em seguida, você pode mover as instruções `await` do bacon e dos ovos até o final do método, antes de servir o café da manhã:



```csharp
Coffee cup = PourCoffee();
Console.WriteLine("coffee is ready");
Task<Egg> eggTask = FryEggs(2);
Task<Bacon> baconTask = FryBacon(3);
Task<Toast> toastTask = ToastBread(2);
Toast toast = await toastTask;
ApplyButter(toast);
ApplyJam(toast);
Console.WriteLine("toast is ready");
Juice oj = PourOJ();
Console.WriteLine("oj is ready");

Egg eggs = await eggTask;
Console.WriteLine("eggs are ready");
Bacon bacon = await baconTask;
Console.WriteLine("bacon is ready");

Console.WriteLine("Breakfast is ready!");
```

O código anterior funciona melhor. Você inicia todas as tarefas assíncronas ao mesmo tempo. Você aguarda cada tarefa somente quando precisar dos resultados. O código anterior pode ser semelhante a um código em um aplicativo Web que faz solicitações de diferentes microsserviços e combina os resultados em uma única página. Você fará todas as solicitações imediatamente e, em seguida, `await` em todas essas tarefas e comporá a página da Web.

## Composição com tarefas

Você prepara tudo para o café da manhã ao mesmo tempo, exceto a torrada. Preparar a torrada é a composição de uma operação assíncrona (torrar o pão) com operações síncronas (adicionar a manteiga e a geleia). A atualização deste código ilustra um conceito importante:

 Importante

A composição de uma operação assíncrona seguida por trabalho síncrono é uma operação assíncrona. Explicando de outra forma, se qualquer parte de uma operação for assíncrona, toda a operação será assíncrona.

O código anterior mostrou que você pode usar objetos [Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task) ou [Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task-1) para manter tarefas em execução. Você `await` em cada tarefa antes de usar seu resultado. A próxima etapa é criar métodos que declarem a combinação de outro trabalho. Antes de servir o café da manhã, você quer aguardar a tarefa que representa torrar o pão antes de adicionar manteiga e geleia. Você pode declarar esse trabalho com o código a seguir:



```csharp
async Task<Toast> makeToastWithButterAndJamAsync(int number)
{
    var plainToast = await ToastBreadAsync(number);
    ApplyButter(plainToast);
    ApplyJam(plainToast);
    return plainToast;
}
```

O método anterior tem o modificador `async` na sua assinatura. Isso sinaliza ao compilador que esse método contém uma instrução `await`; ele contém operações assíncronas. Este método representa a tarefa que torra o pão e, em seguida, adiciona manteiga e geleia. Esse método retorna um [Task](https://docs.microsoft.com/pt-br/dotnet/api/system.threading.tasks.task-1) que representa a composição dessas três operações. O principal bloco de código agora se torna:



```csharp
static async Task Main(string[] args)
{
    Coffee cup = PourCoffee();
    Console.WriteLine("coffee is ready");
    var eggsTask = FryEggsAsync(2);
    var baconTask = FryBaconAsync(3);
    var toastTask = makeToastWithButterAndJamAsync(2);

    var eggs = await eggsTask;
    Console.WriteLine("eggs are ready");
    var bacon = await baconTask;
    Console.WriteLine("bacon is ready");
    var toast = await toastTask;
    Console.WriteLine("toast is ready");
    Juice oj = PourOJ();
    Console.WriteLine("oj is ready");

    Console.WriteLine("Breakfast is ready!");

    async Task<Toast> makeToastWithButterAndJamAsync(int number)
    {
        var plainToast = await ToastBreadAsync(number);
        ApplyButter(plainToast);
        ApplyJam(plainToast);
        return plainToast;
    }
}
```

A alteração anterior ilustrou uma técnica importante para trabalhar com código assíncrono. Você pode compor tarefas, separando as operações em um novo método que retorna uma tarefa. Você pode escolher quando aguardar essa tarefa. Você pode iniciar outras tarefas simultaneamente.