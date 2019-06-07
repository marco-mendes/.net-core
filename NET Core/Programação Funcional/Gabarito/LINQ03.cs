using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQ03
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> animalNames = new List<string>{"fawn", "gibbon", "heron", "ibex", "jackalope"};
            string pattern = "i";

            IEnumerable<string> longAnimalNames =
                animalNames
                .Where(name => name.Contains(pattern))
                .OrderBy(name => name);
        }
    } 
}
