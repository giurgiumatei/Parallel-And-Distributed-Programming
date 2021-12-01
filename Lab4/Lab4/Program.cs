using System.Linq;
using lab4.Domain;
using Lab4.Domain;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var hosts = new[] { "www.cs.ubbcluj.ro/~motogna/LFTC", "www.cs.ubbcluj.ro/~rlupsa/edu/pdp/progs/futures-demo2-cascade1.cs", "www.cs.ubbcluj.ro/~forest" }.ToList();
          //  AsyncSession.Run(hosts);
            TaskSession.Run(hosts);
          // CallbackSession.Run(hosts);
        }
    }
}
