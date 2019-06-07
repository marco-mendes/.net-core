## Conceitos LINQ - Sintaxe de Método

O outro formato possível da LINQ é a Sintaxe de Método. Esta maneira permite fazer tudo o que o método de Query faz, mas de maneira diferente.

Um exemplo de sintaxe de Query:

```csharp
List<string> animalNames = new List<string>
    {"fawn", "gibbon", "heron", "ibex", "jackalope"};
    
// Result: {"heron", "gibbon", "jackalope"}
IEnumerable<string> longAnimalNames =
    from name in animalNames
    where name.Length >= 5
    orderby name.Length
    select name;
```

Agora o mesmo exemplo utilizando sintaxe de Método:

```csharp
List<string> animalNames = new List<string>
    {"fawn", "gibbon", "heron", "ibex", "jackalope"};

IEnumerable<string> longAnimalNames =
    animalNames
    .Where(name => name.Length >= 5)
    .OrderBy(name => name.Length);
```

### Vantagens da Sintaxe de Método

- A sintaxe de Query é automaticamente converttida para a sintaxe de Método durante a compilação (reduz o tempo de build)Query syntax is automatically converted to method syntax at compilation time
- Nem todos os métodos do LINQ podem ser utilizados pela sintaxe de Query.
- É bem mais similar ao código do C#.

## Exercício

Vamos fazer exatamente o exercício anterior, só que agora aplicando a sintaxe de método.

> O seguinte código mostra uma Query que retorna o valor de `inValues` sem modificações. Usando o formato mostrado acima, veja se consegue retornar somente as strings que contém `pattern` (você pode usar o método [`String.Contains()`](https://msdn.microsoft.com/en-us/library/dy85x1sa(v=vs.110).aspx)) e ordená-las por ordem alfabética.

```c#
using System.Collections.Generic;
using System.Linq;

namespace MethodSyntax1
{
    public static class MethodSyntax1
    {
        public static IEnumerable<string> FilterAndSort(IEnumerable<string> inValues, string pattern)
        {
            return inValues
                // LINQ method calls here
                ;
        }
    }
}
```

