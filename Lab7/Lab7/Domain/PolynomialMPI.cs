using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MPI;
using Environment = MPI.Environment;

namespace Lab7.Domain
{
    [Serializable]
    public partial class Polynomial
    {
        // A slave process can respond with an index and the corresponding value at that index.
        // In this way, the master process knows which elements to populate in the "di" array.
        [Serializable]
        internal struct VectorElement
        {
            public int Index;
            public int Value;
        }

        // A slave process can respond with a row, column and value at that coordinate in the "dpq" matrix.
        [Serializable]
        internal struct MatrixElement
        {
            public int Row;
            public int Column;
            public int Value;
        }

        // Splits workload as evenly as possible given a numerator and a denominator.
        // Used to determine the indexes on which MPI slave processes execute.
        // For example, we wish to split the range 1..10 among 3 processes => 1..3, 4..6, 7..10.
        private static IEnumerable<int> DivideEvenly(int numerator, int denominator)
        {
            if (numerator <= 0 || denominator <= 0)
                throw new InvalidOperationException("Invalid division!");

            var div = Math.DivRem(numerator, denominator, out var mod);

            for (var i = 0; i < denominator; i++)
                yield return i < mod ? div + 1 : div;
        }

        // Computes the workload start and workload end indexes on which MPI slave processes will execute.
        // It is necessary to do this because the number of slave processes in most cases is not equal to the number of elements in a range.
        // To be more clear, say we want to parallelize a for with "i" from 1 to 100, but we have only 13 slave processes.
        // Each will get a corresponding range on which it will iterate internally (the "workloadStart" and "workloadEnd" output parameters).
        private static void MpiFor(int forStart, int forEnd, int forIncrement, Communicator communicator,
            out int workloadStart,
            out int workloadEnd)
        {
            var forIterations = (forEnd - forStart) / forIncrement + 1;

            var workers = communicator.Size - 1;
            var workload = DivideEvenly(forIterations, workers).ToArray();

            workloadStart = 0;
            workloadEnd = workload[0] - 1;

            for (var i = 1; i < communicator.Rank; ++i)
            {
                workloadStart += workload[i - 1];
                workloadEnd = workloadStart + workload[i] - 1;
            }
        }

        // Command Prompt: mpiexec -n 8 Karatsuba.exe
        // 8 is the number of processes (must be greater than 1).
        public static void MultiplyKaratsubaMpi(string[] args)
        // Need to pass the arguments with which "Main" was started.
        {
            using (new Environment(ref args))
            {
                var communicator = Communicator.world;

                Polynomial firstPolynomial = null, secondPolynomial = null;
                int n;
                var stopWatch = Stopwatch.StartNew();

                if (communicator.Rank == 0) // By convention the channel with index 0 is the master process.
                {
                    const int degree = 100;
                    firstPolynomial = RandomPolynomial(degree);
                    secondPolynomial = RandomPolynomial(degree);

                    n = degree + 1;

                    var di = new int[n];
                    var dpq = new int[n, n];

                    // Broadcast the polynomials to all slave processes so they know what they are working on.
                    communicator.Broadcast(ref firstPolynomial, 0);
                    communicator.Broadcast(ref secondPolynomial, 0);

                    // Receive the computed element from the "di" vector.
                    for (var i = 1; i <= n; ++i)
                    {
                        var result = communicator.Receive<VectorElement>(Communicator.anySource, 1);
                        di[result.Index] = result.Value;
                    }

                    // Receive the computed element from the "dpq" matrix.
                    for (var i = 1; i <= n * (n - 1) / 2; ++i)
                    {
                        var result = communicator.Receive<MatrixElement>(Communicator.anySource, 2);
                        dpq[result.Row, result.Column] = result.Value;
                    }

                    // TO DO FOR GRADE 10!!! :)
                    var resultingPolynomial = new int[2 * n - 1];
                    resultingPolynomial[0] = di[0];
                    resultingPolynomial[2 * n - 2] = di[n - 1];
                    for (var i = 1; i <= 2 * n - 3; ++i)
                    {
                        for (var p = 0; p <= i; ++p)
                        {
                            var q = i - p;
                            if (p < n && q < n && q > p)
                                resultingPolynomial[i] += dpq[p, q] - (di[p] + di[q]);
                        }

                        if (i % 2 == 0)
                            resultingPolynomial[i] += di[i / 2];
                    }

                    var output = new Polynomial(resultingPolynomial);
                    stopWatch.Stop();
                    Console.WriteLine($"\nResult of mpi multiplication took {stopWatch.Elapsed.TotalMilliseconds} milliseconds and the last coefficient is " +
                                        $"{output.Coefficients[0]}");
                    
                    PrintRegularMultiplication(firstPolynomial, secondPolynomial);

                    PrintKaratsubaIterativeMultiplication(firstPolynomial, secondPolynomial);

                    PrintRegularParallelMultiplication(firstPolynomial, secondPolynomial);

                    PrintKaratsubaRecursiveMultiplication(firstPolynomial, secondPolynomial);
                }
                else // Otherwise it is a slave process.
                {
                    // Receive the broadcasted polynomials.
                    communicator.Broadcast(ref firstPolynomial, 0);
                    communicator.Broadcast(ref secondPolynomial, 0);

                    n = Math.Min(firstPolynomial.Degree, secondPolynomial.Degree) + 1;

                    // The "for" lower and upper bounds.
                    int workloadStart, workloadEnd;

                    MpiFor(0, n - 1, 1, communicator, out workloadStart, out workloadEnd);
                    for (var i = workloadStart; i <= workloadEnd; ++i)
                        communicator.Send(new VectorElement { Index = i, Value = firstPolynomial.Coefficients[i] * secondPolynomial.Coefficients[i] }, 0, 1);

                    MpiFor(0, 2 * n - 3, 1, communicator, out workloadStart, out workloadEnd);
                    for (var i = workloadStart; i <= workloadEnd; ++i)
                        for (var p = 0; p <= i; ++p)
                        {
                            var q = i - p;
                            if (p < n && q < n && q > p)
                                communicator.Send(
                                    new MatrixElement
                                    {
                                        Row = p,
                                        Column = q,
                                        Value =
                                            (firstPolynomial.Coefficients[p] + firstPolynomial.Coefficients[q]) *
                                            (secondPolynomial.Coefficients[p] + secondPolynomial.Coefficients[q])
                                    }, 0, 2);
                        }
                }
            }
        }
    }
}
