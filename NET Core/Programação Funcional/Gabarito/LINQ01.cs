using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LINQ01
{
    public static class LambdaExpressions1
    {
        // Write a lambda expression that will return the next number after
        // the provided integer
        public static Func<int, int> GetNextNumber = number => number + 1;
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
