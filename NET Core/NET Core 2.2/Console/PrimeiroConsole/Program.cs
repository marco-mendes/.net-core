using System;
using Microsoft.Extensions.CommandLineUtils;

namespace PrimeiroConsole
{
    class Program
    {
        static void Main(string[] args)
        {
          var app = new CommandLineApplication();
          app.Name = "Fibonacci como Argumento";
          app.Description = ".NET Core console - Fibonacci e Argumentos";

          app.HelpOption("-?|-h|--help");

          var NameOption = app.Option("-n|--nome<Nome>",
                                       "Nome do usuario",
                                       CommandOptionType.SingleValue);
          
          var FibOption = app.Option("-f|--fib<Fibonacci>",
                                       "Quantidade de Numeros",
                                       CommandOptionType.SingleValue);

          app.OnExecute(() => {
            if (NameOption.HasValue()) {
              Console.WriteLine("Seu nome eh: {0}", NameOption.Value());
            }
            if (FibOption.HasValue()) {
              var generator = new FibonacciGenerator();
              foreach (var digit in generator.Generate(System.Convert.ToInt32(FibOption.Value())))
              {
                Console.WriteLine(digit);
              }
            }
            else {
              app.ShowHint();
            }

            return 0;
          });

          app.Command("fibonacci", (command) => {
            command.Description = "Uma descricao qualquer.";
            command.HelpOption("-?|-h|--help");

            command.OnExecute(() => {
              Console.WriteLine("fibonacci finalizado.");
              return 0;
            });
          });

          app.Execute(args);
        }
    }
}