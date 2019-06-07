## LINQ - Sintaxe de Query

Quando o assunto é funcionalidades do LINQ, há dois tópicos principais: a **sintaxe de query** e **sintaxe de método**

A sintaxe de query tem um estilo sintático bem diferente da linguagem em si. Um exemplo:

```csharp
using System.Linq;

   ...

List<string> animalNames = new List<string>
    {"fawn", "gibbon", "heron", "ibex", "jackalope"};
    
// Result: {"heron", "gibbon", "jackalope"}
IEnumerable<string> longAnimalNames =
    from name in animalNames
    where name.Length >= 5
    orderby name.Length
    select name;
```

Para aqueles com familiaridade com a sintaxe SQL, é possível encontrar algumas similaridades entre elas, embora não sejam a mesma coisa. O exemplo acima recolhe todas as strings na lista `animalNames` que tem pelo menos 5 caracteres de comprimento, ordedando pelo tamanho.

> **NOTA:** O exemplo acima contém `using System.Linq;`. Isso é essencial ao utilizar o LINQ, tanto para Querys quanto para Métodos.

### Vantagens da Sintaxe de Query

- [MSDN documentation](https://msdn.microsoft.com/en-us/library/bb397947.aspx) - "*many people find query syntax simpler and easier to read*."

### Um exemplo insado de LINQ:

Só para você ter uma ideia de o que é possível fazer com LINQ:

[Taking LINQ to Objects to Extremes: A fully LINQified RayTracer](https://blogs.msdn.microsoft.com/lukeh/2007/10/01/taking-linq-to-objects-to-extremes-a-fully-linqified-raytracer/)

Não precisa entender (ou decifrar)! É só uma amostra do que a linguagem pode fazer!

## Exercício

O seguinte código mostra uma Query que retorna o valor de `inValues` sem modificações. Usando o formato mostrado acima, veja se consegue retornar somente as strings que contém `pattern` (você pode usar o método [`String.Contains()`](https://msdn.microsoft.com/en-us/library/dy85x1sa(v=vs.110).aspx)) e ordená-las por ordem alfabética.

```c#
using System.Collections.Generic;
using System.Linq;

namespace QuerySyntax1
{
    public static class QuerySyntax1
    {
        public static IEnumerable<string> FilterAndSort(IEnumerable<string> inValues, string pattern)
        {
            return from value in inValues
                // LINQ instructions here
                select value;
        }
    }
}
```

