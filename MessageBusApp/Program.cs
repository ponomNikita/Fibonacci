using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace MessageBusApp
{
    class Program
    {
        private static ILog _logger = LogManager.GetLogger("MessageBusProgram");
        private const int MAX_ITERATION_COUNT = 46;
        private const int MIN_ITERATION_COUNT = 0;

        static void Main(string[] args)
        {
            FibonacciService service = null;
            try
            {
                service = new FibonacciService();
                do
                {
                    Console.WriteLine("Enter count iterations (or \"quit\" to exit)");
                    Console.Write("> ");
                    string iterationsCountStr = Console.ReadLine();

                    if ("quit".Equals(iterationsCountStr, StringComparison.OrdinalIgnoreCase))
                        break;

                    int iterationsCount;
                    if (!int.TryParse(iterationsCountStr, out iterationsCount))
                        continue;

                    if (iterationsCount < MIN_ITERATION_COUNT || iterationsCount > MAX_ITERATION_COUNT)
                    {
                        Console.Out.WriteLine(string.Format("Count of iterations must be between {0} and {1}",
                            MIN_ITERATION_COUNT, MAX_ITERATION_COUNT));
                        continue;
                    }

                    ulong result = service.GetNumber(iterationsCount);
                    Console.Out.WriteLine(string.Format("Ответ: {0}", result));

                } while (true);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Произошла ошибка: {0}", ex);
            }
            finally
            {
                if (service != null)
                    service.Dispose();

                Console.ReadLine();
            }
        }
    }
}
