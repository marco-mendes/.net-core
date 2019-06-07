using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQ04
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> palavras = new List<string> { "Mocassim", "Maratona", "Ritmo", "Licor", "Mocassim", "Mocassim", "Tango", "Tenor", "Peso", "Quatro" };
            
            IEnumerable<string> filtradas =
                palavras
                .Where(name => name.Length > 5)
                .OrderBy(name => name)
                .Distinct();
        
            foreach(var pal in filtradas) {
                Console.WriteLine(pal);
            }
            
        }
    }
}
