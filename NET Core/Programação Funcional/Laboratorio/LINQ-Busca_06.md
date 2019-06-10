## Criando Relações - Parte 5/5

Nesta parte, vamos criar os controladores para cada uma de nossas entidades, expondo uma interface para o mundo externo.

Nesta parte, todas as criações e alterações - provavelmente - serão feitas no projeto `Application`. Caso queira que o comando `dotnet` execute seu projeto novamente sempre que um arquivo for modificado, basta executar o comando:

```bash
dotnet watch run
```

Desta forma, o `dotnet` verifica todos os arquivos do projeto e sempre que há uma alteração, o projeto é parado e executado novamente, evitando a necessidade de finalizar o processo e iniciar com `dotnet run`.



#### Controlador de Fabricante

