using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciSolver
{
    public interface IFibonacciSolver
    {
        /// <summary>
        /// Получает следующий член последовательности фибоначи
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        ulong GetNext(ulong x);
    }
}
