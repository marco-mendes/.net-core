using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQ02
{
    public static class QuerySyntax1
   {
        public static IEnumerable<string> FilterAndSort(IEnumerable<string> inValues, string pattern)
        {
            return from value in inValues
                where value.Contains(pattern)
                orderby value
                select value;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> animalNames = new List<string>{"fawn", "gibbon", "heron", "ibex", "jackalope"};
            foreach(var animal in QuerySyntax1.FilterAndSort(animalNames, "i")) {
                Console.WriteLine(animal);
            }
        }
    }
}
