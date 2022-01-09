using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab7.Domain
{
    public partial class Polynomial
    {
        // Parallel multiplication.
        public static Polynomial MultiplyRegularParallel(Polynomial a, Polynomial b)
        {
            var coefficients = new int[a.Degree + b.Degree + 1];

            Parallel.For(0, a.Degree + 1, i =>
            {
                for (var j = 0; j <= b.Degree; ++j)
                    Interlocked.Add(ref coefficients[i + j], a.Coefficients[i] * b.Coefficients[j]);
                // Cheapest method for atomic addition of two integers.
            });

            return new Polynomial(coefficients);
        }

        // Parallel Karatsuba multiplication for two polynomials.
        public static Polynomial MultiplyKaratsubaRecursive(Polynomial x, Polynomial y)
        {
            const int threshold = 100;

            // If the degree of at least one polynomial is below a given threshold, perform regular serial multiplication.
            if (x.Degree <= threshold || y.Degree <= threshold)
                return x * y;

            // For polynomials, the split point is half the maximum degree of the two.
            var length = Math.Max(x.Degree + 1, y.Degree + 1);
            length = length / 2;

            // Split the polynomials into high and low parts respectively.

            var low1 = new int[length];
            Array.Copy(x.Coefficients, 0, low1, 0, low1.Length);

            var high1 = new int[x.Degree + 1 - length];
            Array.Copy(x.Coefficients, length, high1, 0, high1.Length);

            var low2 = new int[length];
            Array.Copy(y.Coefficients, 0, low2, 0, low2.Length);

            var high2 = new int[y.Degree + 1 - length];
            Array.Copy(y.Coefficients, length, high2, 0, high2.Length);

            var h1 = new Polynomial(high1);
            var l1 = new Polynomial(low1);
            var h2 = new Polynomial(high2);
            var l2 = new Polynomial(low2);

            // Apply divide and conquer recursion.
            var t0 = Task<Polynomial>.Factory.StartNew(() => MultiplyKaratsubaRecursive(l1, l2));
            var t1 = Task<Polynomial>.Factory.StartNew(() => MultiplyKaratsubaRecursive(l1 + h1, l2 + h2));
            var t2 = Task<Polynomial>.Factory.StartNew(() => MultiplyKaratsubaRecursive(h1, h2));

            Task.WaitAll(t0, t1, t2);

            var z0 = t0.Result;
            var z1 = t1.Result;
            var z2 = t2.Result;

            // Compute final result.
            return (z2 << (2 * length)) + ((z1 - z2 - z0) << length) + z0;
        }
    }
}
