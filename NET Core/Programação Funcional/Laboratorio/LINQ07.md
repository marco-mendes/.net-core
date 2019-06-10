### Recursividade

> Um método comum de simplificação consiste em dividir um problema em subproblemas do mesmo tipo. Como técnica de programação, isto se denomina divisão e conquista, e constitui a chave para o desenvolvimento de muitos algoritmos importantes, bem como um elemento fundamental do paradigma de programação dinâmica.
>
> Praticamente todas as linguagens de programação usadas hoje em dia permitem a especificação direta de funções e procedimentos recursivos. Quando uma função é invocada, o computador (na maioria das linguagens sobre a maior parte das arquiteturas baseadas em pilhas) ou a implementação da linguagem registra as várias instâncias de uma função (em muitas arquiteturas, usa-se uma pilha de chamada, embora outros métodos possam ser usados). Reciprocamente, toda função recursiva pode ser transformada em uma função iterativa usando uma pilha.

Um exemplo de função recursiva que calcula a soma de todos os inteiros entre os números `n` e `m`.

```c#
class Program
{
    public static int CalculateSumRecursively(int n, int m)
    {
        int sum = n;
 
        if(n < m)
        {
            n++;
            return sum += CalculateSumRecursively(n, m);
        }
 
        return sum;
   }
 
    static void Main(string[] args)
    {
        Console.WriteLine("Enter number n: ");
        int n = Convert.ToInt32(Console.ReadLine());
 
        Console.WriteLine("Enter number m: ");
        int m = Convert.ToInt32(Console.ReadLine());
 
        int sum = CalculateSumRecursively(n, m);
 
        Console.WriteLine(sum);
 
        Console.ReadKey();
    }
}

```

**Exercício**

Dado uma lista de produtos, comoa disponibilizada no [Program.cs](utils/Program.cs), monte uma função recursiva para calcular o total de preço de todos os produtos.

> Note que esta tarefa também pode ser realizada através de uma função Lambda.



### Pogramação Assíncrona

Um exemplo de programa que executa uma chamada GET ao site whatthecommit.com e retorna para o usuário a resposta.

```c#
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Assincrono
{
    class GetPageContent {
        private const string _address = "https://whatthecommit.com/index.txt";

        public async Task<string> GetContent(string print)
        {
            var result = await GetAsync(_address);
            Console.WriteLine(print);
            return result.ToString();
        }
        public async Task<string> GetContentTAsync(string print)
        {
            var result = GetAsync(_address);
            Console.WriteLine(print);
            return result.ToString();
        }
        private async Task<string> GetAsync(string uri)
        {
            HttpClient client = new HttpClient();

            var content = await client.GetStringAsync(uri);

            return content;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            GetPageContent getter = new GetPageContent();

            var result1 = getter.GetContentTAsync("Primeiro");
            Console.WriteLine(result1.Result);
            var result2 = getter.GetContent("Segundo");
            Console.WriteLine(result2.Result);
            
        }
    }
}
```

