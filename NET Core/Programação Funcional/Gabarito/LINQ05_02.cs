using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace LINQ0502
{
     class Produto {
            public string Nome {get; set;}
            public double Preco {get; set;}
            public int CategoriaId {get;set;}
            public DateTime Validade {get;set;}
        public Produto (string nome, string validade, double preco, int categoriaId) {
            Nome = nome;
            Validade = DateTime.ParseExact(validade, "dd/MM/yyyy", null);
            Preco = preco;
            CategoriaId = categoriaId;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Produto> produtos = new List<Produto>{new Produto( "Isabella", "30/03/2023", 62.3, 5 ), new Produto( "Jon", "29/12/2024", 55.1, 6 ), new Produto( "Mollie", "15/11/2024", 148.23, 0 ), new Produto( "Caleb", "09/03/2020", 90.8, 4 ), new Produto( "Rachel", "26/05/2025", 198.4, 4 ), new Produto( "Anthony", "21/01/2022", 168.77, 7 ), new Produto( "Peter", "23/10/2023", 58.6, 8 ), new Produto( "Mittie", "16/05/2022", 47.83, 2 ), new Produto( "Hettie", "05/05/2020", 196.5, 6 ), new Produto( "Bernard", "21/12/2022", 178.7, 9 ), new Produto( "Anthony", "23/09/2024", 193.1, 7 ), new Produto( "Alejandro", "10/02/2020", 132, 5 ), new Produto( "Elijah", "09/08/2020", 29.79, 1 ), new Produto( "Ina", "24/07/2023", 13.9, 8 ), new Produto( "Agnes", "04/12/2021", 130.8, 9 ), new Produto( "Susie", "16/12/2024", 155.4, 8 ), new Produto( "Dylan", "01/03/2021", 7.2, 5 ), new Produto( "Philip", "21/05/2023", 89.2, 7 ), new Produto( "Henry", "26/08/2023", 4.5, 0 ), new Produto( "Jorge", "08/09/2024", 121.7, 1 ), new Produto( "Howard", "28/12/2023", 194.29, 3 ), new Produto( "Susan", "07/05/2022", 44.05, 8 ), new Produto( "Sam", "22/11/2022", 20.4, 1 ), new Produto( "Oscar", "26/08/2022", 70.14, 0 ), new Produto( "Viola", "03/01/2022", 89.35, 2 ), new Produto( "Francis", "25/02/2021", 116.64, 5 ), new Produto( "Hilda", "28/04/2024", 175.6, 2 ), new Produto( "Sarah", "01/12/2024", 178.5, 8 ), new Produto( "Austin", "21/06/2023", 85.3, 8 ), new Produto( "Alejandro", "22/11/2019", 12.33, 3 ), new Produto( "Eugenia", "01/03/2020", 34.8, 10 ), new Produto( "Norman", "16/11/2020", 55.9, 9 ), new Produto( "Lilly", "18/03/2020", 173.77, 2 ), new Produto( "Leona", "25/06/2022", 42.1, 9 ), new Produto( "Ronnie", "22/02/2024", 137.18, 0 ), new Produto( "Allie", "13/02/2022", 69.61, 10 ), new Produto( "Sophie", "20/12/2024", 48.9, 3 ), new Produto( "Rose", "14/12/2022", 79.14, 1 ), new Produto( "Eula", "24/01/2021", 99.4, 5 ), new Produto( "Ralph", "01/04/2020", 90.51, 9 ), new Produto( "Nicholas", "04/09/2024", 5.7, 5 ), new Produto( "Norman", "26/06/2024", 159.31, 4 ), new Produto( "Blanche", "09/02/2023", 10.3, 0 ), new Produto( "Larry", "20/05/2021", 95.1, 5 ), new Produto( "Ricky", "09/09/2024", 1.8, 9 ), new Produto( "Eula", "31/03/2021", 82.8, 9 ), new Produto( "David", "11/01/2022", 145, 1 ), new Produto( "Dale", "15/12/2021", 97, 4 ), new Produto( "Eugenia", "05/04/2024", 23.22, 9 ), new Produto( "Cory", "07/10/2022", 172.9, 7 ), new Produto( "Lucinda", "21/09/2025", 176.53, 0 ), new Produto( "Hulda", "13/08/2025", 66.6, 10 ), new Produto( "Glenn", "25/04/2021", 107.7, 7 ), new Produto( "Amanda", "09/06/2024", 4.52, 1 ), new Produto( "Roger", "30/07/2024", 21.7, 0 ), new Produto( "Juan", "27/03/2023", 48.1, 5 ), new Produto( "Lillie", "13/03/2021", 109.38, 3 ), new Produto( "Eva", "30/01/2025", 171.43, 0 ), new Produto( "Katharine", "26/06/2020", 127.53, 3 ), new Produto( "Kathryn", "10/03/2023", 70.49, 0 ), new Produto( "Jordan", "22/03/2021", 13, 2 ), new Produto( "James", "02/06/2022", 144.6, 9 ), new Produto( "Jeremiah", "26/08/2020", 114, 6 ), new Produto( "Jonathan", "27/09/2022", 63.75, 2 ), new Produto( "Timothy", "21/05/2022", 63.41, 2 ), new Produto( "Inez", "24/06/2021", 19.91, 3 ), new Produto( "Ernest", "14/01/2024", 70.5, 7 ), new Produto( "Maria", "20/12/2020", 92.16, 6 ), new Produto( "Charlotte", "19/01/2020", 172.39, 6 )};
            
            var queryGroupMax = produtos
                .GroupBy(pdt => pdt.CategoriaId)
                .Select(CategoriaGroup => new
                {
                    Categoria = CategoriaGroup.Key,
                    Min = CategoriaGroup.Select(pdt => pdt.Preco).Min(),
                    Max = CategoriaGroup.Select(pdt => pdt.Preco).Max(),
                    Media = CategoriaGroup.Select(pdt => pdt.Preco).Average(),
                });

                Console.WriteLine($"Number of groups = {queryGroupMax.Count()}");

                foreach (var item in queryGroupMax)
                {
                    Console.WriteLine($"Categoria: {item.Categoria}");
                    Console.WriteLine($"Max={item.Max}");
                    Console.WriteLine($"Min={item.Min}");
                    Console.WriteLine($"Media={item.Media}\n");
                }

        }
    }
}