using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciSolver
{
    public static class FibonacciSolverFactory
    {
        public static IFibonacciSolver GetSolver()
        {
            return new FibonacciSolver();
        }
    }
}
