## Construindo imagens com múltiplos estágios

Durante o processo de build de uma aplicação, pensando em âmbito de desenvolvimento ágil, é interessante que as transições de estado (dev, teste e produção) sejam mais fluidas possíveis. Para isso, é necessário abstrair condições específicas de cada ambiente, como versões de bibliotecas, frameworks, etc.

Nos laboratórios anteriores, utilizamos os comandos `dotnet publish` e `dotnet buid` para fazer a compilação da nossa aplicação na máquina local, utilizando o próprio SDK instalado e só depois importá-la para uma imagem Docker. Dessa forma, se cada máquina de desenvolvimento tiver uma versão diferente ou parâmetros de configuração diferentes, o resultado final não seria constante.

Como já vimos anteriormente, o docker consegue garantir que o ambiente de execução da aplicação seja padrão, como configurado no Dockerfile. E agora está na hora de demonstrarmos como utilizar o Docker para realizar todo o processo de build, testes e publish do código.

### O que é a build de vários estágios?

Nas versões superiores à 17.05 do Docker, há a possibilidade de escrevermos um Dockerfile que utiliza várias imagens para fazer a build e release do código. Para entender melhor a funcionalidade, vamos analisar uma situação problema e como ela pode ser resolvida através da build de vários estágios.

##### Problema

Em um cenário hipotético em que temos uma aplicação chamada `Terminal`, escrita .Net Core 2.2, e como requisitos:

- Fazer a build em ambiente controlado (Imagem Docker) 
  - Cada desenvolvedor tem uma máquina livre, então a build pode ter dependências que poderiam quebrar em servidores de teste e produção.
- Distribuí-las através de uma imagem Docker
  - Para alta escalabilidade e fácil manutenção e deploy.

podemos realizar a build utilizando como base a imagem do SDK do .Net Core 2.2.

Para isso, precisaríamos copiar todo o código para dentro do container, executar os comandos de build e especificar o que deve ser executado com a keyword `ENTRTYPOINT`.

Dessa forma, o Dockerfile ficaria:

```dockerfile
FROM microsoft/dotnet:2.2-sdk
WORKDIR /src

# Copiar solução e restaurar dependencias
COPY . ./
RUN dotnet restore

# Publicando aplicação console em pasta Terminal/out
RUN dotnet publish "Terminal/Terminal.csproj" -c Release -o out

ENTRYPOINT ["dotnet", "Terminal.dll"]
```

Ao gerar a imagem você percebe que, embora sua aplicação tenha poucos MB, a imagem final ficou com 1,75GB! O motivo disso é que a imagem do SDK contém todos os binários, rotinas e biblitecas do .Net 2.2 necessárias para compilar o código, se tornando pesada em termos de espaço. Mas para servir sua aplicação você não precisa de tudo isso (afinal ela já se encontra compilada), basta uma imagem leve que tenha somente o runtime do .Net 2.2.



##### Solução

> Com o multi-stage, você usa várias instruções FROM no seu Dockerfile, cada instrução FROM pode usar uma imagem base diferente, e cada uma delas começa um novo estágio da compilação. Você pode copiar artefatos de um estágio para outro, deixando para trás tudo que você não quer na imagem final. 
>
> [Mundo Docker](https://www.mundodocker.com.br/docker-multi-stage-builds/) 

Para deixar o processo mais fácil, o Docker permite que várias imagens sejam utilizadas durante o processo de build. Dessa forma, é possível utilizar imagens grandes e com livres permissões para realizar a build (incluindo testes e gerência de dependências) e publicar os binários em imagens leves, exclusivas para a execução da aplicação.

Então, um exemplo de Dockerfile para a build e publicação da aplicação `Terminal`seria:

```dockerfile
# Utilizando o sdk como ambiente de build
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src

# Copiar solução e restaurar dependencias
COPY . ./
RUN dotnet restore

# Publicando aplicação console em pasta Terminal/out
RUN dotnet publish "Terminal/Terminal.csproj" -c Release -o out

# Criação da imagem final - com apenas o runtime
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /src/Terminal/out .
ENTRYPOINT ["dotnet", "Terminal.dll"]
```

Dessa forma, conseguimos garantir a consistência de todos os passos da build e distribuição sem depender de variáveis externas, entregando um executável limpo. 

Além da consistência, a imagem final pode ficar na faixa de 300MB, muito diferente dos 1,75GB anteriores, enquanto mantém a mesma função.

Agora que a build de múltiplos estágios foi apresentada, vamos ver como realizar os testes e a publicação de uma aplicação, tendo como base o código gerado durante o laboratório de xUnit.



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
dotnet sln unit-testing-using-dotnet-test.sln add Terminal/Terminal.csproj
```

Dessa forma, estaremos especificando que o projeto do Terminal faz referência ao projeto do PrimeService através da linha de comando.

Alteramos então o código de Terminal/Program.cs para o seguinte:

```c#
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
```

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