using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciSolver
{
    internal class FibonacciSolver : IFibonacciSolver
    {
        public ulong GetNext(ulong x)
        {
            var a = Math.Sqrt(5 * x * x + 4);
            var b = Math.Sqrt(5 * x * x - 4);

            ulong res;

            if (!ulong.TryParse(a.ToString(CultureInfo.InvariantCulture), out res))
            {
                if (!ulong.TryParse(b.ToString(CultureInfo.InvariantCulture), out res))
                    throw new Exception("Ни один корень не извлекся нацело");
            }

            res = (x + res) / 2;

            return res;
        }
    }
}
