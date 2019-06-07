## Métodos - Alguns Exemplos

## [Distinct()](https://msdn.microsoft.com/en-us/library/bb348436(v=vs.110).aspx)

O método `Distinct()` funciona da mesma forma que a diretiva `DISTINCT` no SQL. Ele retorna uma nova sequência contendo todos os elementos passados como parâmetro que são diferentes.

Por exemplo:

```csharp
List<int> ints = new List<int> { 1, 2, 4, 8, 4, 2, 1 };
// Will contain { 1, 2, 4, 8 }
IEnumerable<int> result = ints.Distinct();
```

## [Intersect()](https://msdn.microsoft.com/en-us/library/bb460136(v=vs.110).aspx)

`Intersect()` retorna uma nova sequência que contém todos os elementos que são comuns entre os parâmetros passados. 

Por exemplo:

```csharp
List<int> ints = new List<int> { 1, 2, 4, 8, 4, 2, 1 };
List<int> filter = new List<int> { 1, 1, 2, 3, 5, 8 };
// Will contain { 1, 2, 8 }
IEnumerable<int> result = ints.Intersect(filter);
```

## [Where()](https://msdn.microsoft.com/en-us/library/bb534803(v=vs.110).aspx)

`Where()`  retorna uma nova sequência contendo todos os elementos do parâmetro que atendem ao critério especificado.

> **NOTA:** LINQ está mostrando um pouco das raízes SQL. Embora o método `Where()` receba seu nome baseado na sintaxe SQL, ele se comporta mais como a função `filter()` do  Java, JavaScript, Python, etc.

O critério é passado ao  `Where()` como um método delegado, que recebe um parâmetro genérico de tipo  `T` (where `T` is the data type of the elements in the `IEnumerable<T>` sequence) e retorna um `bool` , indicando se o elemento passado deve ou não ser incluído na sequência de retorno.

```csharp
List<int> ints = new List<int> { 1, 2, 4, 8, 4, 2, 1 };
// Will contain { 2, 4, 4, 2 }
IEnumerable<int> result = ints.Where(theInt => theInt == 2 || theInt == 4);
```

## Exercício

Neste exercício, devemos motar um método que retorna palavras de uma sequência que têm comprimento maior que 5 e são distintas.

```
List<string> palavras = new List<string> { "Mocassim", "Maratona", "Ritmo", "Licor", "Mocassim", "Mocassim", "Tango", "Tenor", "Peso", "Quatro" };
```

