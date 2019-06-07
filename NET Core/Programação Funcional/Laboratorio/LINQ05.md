### LINQ - Filtrando, Agrupando e Realizando SubQuerys

---

Para este exercício e os próximos, na pasta utils, há um arquivo [Program.cs](utils/Pogram.cs) com a definição de uma classe Produto com um array já populado com valores gerados automaticamente, para facilitar a implementação e testes.

### 1. Criar um grupo aninhado

O exemplo a seguir mostra como criar grupos aninhados em uma expressão de consulta LINQ. Cada grupo que é criado de acordo com o ano do aluno ou nível de ensino, é subdividido em grupos com base nos nomes das pessoas.

```c#
public void QueryNestedGroups()
{
    var queryNestedGroups =
        from student in students
        group student by student.Year into newGroup1
        from newGroup2 in
            (from student in newGroup1
             group student by student.LastName)
        group newGroup2 by newGroup1.Key;

    // Three nested foreach loops are required to iterate 
    // over all elements of a grouped group. Hover the mouse 
    // cursor over the iteration variables to see their actual type.
    foreach (var outerGroup in queryNestedGroups)
    {
        Console.WriteLine($"DataClass.Student Level = {outerGroup.Key}");
        foreach (var innerGroup in outerGroup)
        {
            Console.WriteLine($"\tNames that begin with: {innerGroup.Key}");
            foreach (var innerGroupElement in innerGroup)
            {
                Console.WriteLine($"\t\t{innerGroupElement.LastName} {innerGroupElement.FirstName}");
            }
        }
    }
}
```



**Exercício**

A tarefa então é: dado o conjunto de dados no arquivo [Program.cs](utils/Pogram.cs), crie uma query que agrupe os produtos dados através do ID da categoria e então pelo Ano da data de validade.





### 2. Criando uma subconsulta

O exemplo a seguir mostra um exemplo de como criar uma consulta que ordena os dados de origem em grupos e, em seguida, realizar uma subconsulta em cada grupo individualmente. A técnica básica em cada exemplo é agrupar os elementos de origem usando uma *continuação* chamada `newGroup` e, em seguida, gerar uma nova subconsulta de `newGroup`. Essa subconsulta é executada em cada novo grupo criado pela consulta externa. Observe que, nesse exemplo específico, a saída final não é um grupo, mas uma sequência simples de tipos anônimos.

```c#
public void QueryMaxUsingMethodSyntax()
{
    var queryGroupMax = students
        .GroupBy(student => student.Year)
        .Select(studentGroup => new
        {
            Level = studentGroup.Key,
            HighestScore = studentGroup.Select(student2 => student2.ExamScores.Average()).Max()
        });

    int count = queryGroupMax.Count();
    Console.WriteLine($"Number of groups = {count}");

    foreach (var item in queryGroupMax)
    {
        Console.WriteLine($"  {item.Level} Highest Score={item.HighestScore}");
    }
}
```



**Exercício**

Dado o mesmo array de produtos do [Program.cs](utils/Pogram.cs), agora vamos realizar um agrupamento por categoria e então buscar o preço do produto mais caro da categoria, o mais barato e finalmente a média.





### 3. Filtros predicados

Em alguns casos, você não sabe até o tempo de execução quantos predicados precisa aplicar aos elementos de origem na cláusula `where`. Uma maneira de especificar dinamicamente vários filtros de predicados é usar o método [Contains](https://docs.microsoft.com/pt-br/dotnet/api/system.linq.enumerable.contains), conforme mostrado no exemplo a seguir. O exemplo é construído de duas maneiras. Primeiro, o projeto é executado filtrando valores que são fornecidos no programa. Em seguida, o projeto é executado novamente usando a entrada fornecida em tempo de execução.

Um exemplo de uma função que busca por ID é mostrada abaixo:

```c#
 static void QueryByID(string[] ids)
    {
        var queryNames =
            from student in students
            let i = student.ID.ToString()
            where ids.Contains(i)
            select new { student.LastName, student.ID };

        foreach (var name in queryNames)
        {
            Console.WriteLine($"{name.LastName}: {name.ID}");
        }
    }
```



**Exercício**

Dado o mesmo array de produtos do [Program.cs](utils/Pogram.cs) e com base no exemplo acima, vamos realizar uma busca no array onde buscamos produtos com categorias {5,6,7}.

