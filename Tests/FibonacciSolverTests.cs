using System;
using FibonacciSolver;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FibonacciSolverTests
    {
        private readonly IFibonacciSolver _solver = FibonacciSolverFactory.GetSolver();

        [TestCase((ulong)5, ExpectedResult = (ulong)8, TestName = "FibonacciSolver->GetNext when x = 5")]
        [TestCase((ulong)8, ExpectedResult = (ulong)13, TestName = "FibonacciSolver->GetNext when x = 8")]
        [TestCase((ulong)1, ExpectedResult = (ulong)2, TestName = "FibonacciSolver->GetNext when x = 1")]
        [TestCase((ulong)0, ExpectedResult = (ulong)1, TestName = "FibonacciSolver->GetNext when x = 0")]
        public ulong FibonacciSolverGetNext(ulong x)
        {
            return _solver.GetNext(x);
        }
    }
}
