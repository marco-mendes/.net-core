using System;
using Prime.Services;

namespace Terminal
{
    public class Program {

        static void Main(string[] args)
        {
            var _primeService = new PrimeService();
            var counter = 0;
            var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
            while(max == -1 || counter < max)
            {
                counter++;
                Console.WriteLine($"Counter: {counter}");
                Console.WriteLine($"Is Prime: {_primeService.IsPrime(counter)}");
                System.Threading.Tasks.Task.Delay(1000).Wait();
            }
        }
    }
}
