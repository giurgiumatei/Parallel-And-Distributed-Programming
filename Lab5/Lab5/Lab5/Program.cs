using System;
using System.Diagnostics;
using Lab5.Domain;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            const int degree = 1000;
            var firstPolynomial = Polynomial.RandomPolynomial(degree);
            var secondPolynomial = Polynomial.RandomPolynomial(degree);

            Console.WriteLine($"First polynomial is: {firstPolynomial}");
            Console.WriteLine($"Second polynomial is: {secondPolynomial}");

            var stopWatch = Stopwatch.StartNew();
            var regularMultiplicationResult = Polynomial.Multiply(firstPolynomial, secondPolynomial);
            stopWatch.Stop();
            //Console.WriteLine($"\nResult of regular multiplication is:\n {regularMultiplicationResult}\n and it took {stopWatch.Elapsed.TotalMilliseconds} milliseconds");
            Console.WriteLine($"\nResult of regular multiplication took {stopWatch.Elapsed.TotalMilliseconds} milliseconds {regularMultiplicationResult.Coefficients[0]}");

            stopWatch = Stopwatch.StartNew();
            var regularParallelMultiplicationResult = Polynomial.MultiplyRegularParallel(firstPolynomial, secondPolynomial);
            stopWatch.Stop();
            //Console.WriteLine($"\nResult of regular parallel multiplication is:\n {regularParallelMultiplicationResult}\n and it took {stopWatch.Elapsed.TotalMilliseconds} milliseconds");
            Console.WriteLine($"\nResult of regular parallel multiplication took {stopWatch.Elapsed.TotalMilliseconds} milliseconds {regularParallelMultiplicationResult.Coefficients[0]}");

            stopWatch = Stopwatch.StartNew();
            var karatsubaIterativeResult = Polynomial.MultiplyKaratsubaIterative(firstPolynomial, secondPolynomial);
            stopWatch.Stop();
            //Console.WriteLine($"\nResult of karatsuba iterative multiplication is:\n {karatsubaIterativeResult}\n and it took {stopWatch.Elapsed.TotalMilliseconds} milliseconds");
            Console.WriteLine($"\nResult of karatsuba iterative multiplication took {stopWatch.Elapsed.TotalMilliseconds} milliseconds {karatsubaIterativeResult.Coefficients[0]}");

            stopWatch = Stopwatch.StartNew();
            var karatsubaRecursiveResult = Polynomial.MultiplyKaratsubaRecursive(firstPolynomial, secondPolynomial);
            stopWatch.Stop();
            //Console.WriteLine($"\nResult of karatsuba recursive multiplication is:\n {karatsubaRecursiveResult}\n and it took {stopWatch.Elapsed.TotalMilliseconds} milliseconds");
            Console.WriteLine($"\nResult of karatsuba recursive multiplication took {stopWatch.Elapsed.TotalMilliseconds} milliseconds {karatsubaRecursiveResult.Coefficients[0]}");
        }
    }
}
