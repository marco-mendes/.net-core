## Início - Expressões Lambda

Uma expressção lâmbda é um jeito conveniente de definir uma função anônima que pode ser passada como parâmetro para um método. Muitos métodos LINQ utilizam uma função como parâmetro. Um exemplo de uma expressão lâmbda:

```csharp
Func<int, int> multiplicaPorCinco = num => num * 5;
// Retorna 35
int result = multiplicaPorCinco(7);
```

A expressão `num => num * 5` é uma expressão lâmbda. O operador `=>` é chamado de "operador lâmbda". No exemplo acima,  `num` é um parâmetro da função anônima.

### Parâmetro(s)

Perceba que na nossa função anterior, `num` não possui um tipo definido. O compilador sempre irá inferir o tipo do parâmetro das funções lambda através do contexto. Neste contexto, o compilador iria armazenar a função em uma variável do tipo `Func<int,int>`. Isso significa que ela recebe um inteiro como parâmetro e retorna um interio como resultado.

Você também pode criar expressões lâmbda com mais de um parâmetro:

```csharp
Func<int, int, int> multiplyTwoNumbers = (a, b) => a * b;
// Returns 35
int result = multiplyTwoNumbers(7, 5);
```

### Retorno

Perceba também que não há um `return` definido. Expressões lâmbda de uma linha não precisam explicitar a palavra `return`. Caso haja mais de uma linha, o `return` deve ser utilizado:

```csharp
Func<int, int> multiplyByFive = num =>
{
    int product = num * 5;
    return product;
};
// Returns 35
int result = multiplyByFive(7);
```

## Exercício

Neste simples exercício, escreva uma expressão lâmbda que retorna o valor passado como parâmetro + 1.

```c#
using System;

namespace Background1
{
    public static class LambdaExpressions1
    {
        // Write a lambda expression that will return the next number after
        // the provided integer
        public static Func<int, int> GetNextNumber = ??? => ???;
    }
}
```

