using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lab3.Domain;

namespace Lab3
{
    public class Program
    {
        static void Main()
        {
            int numberOfRows = 5;
            int numberOfColumns = 5;
            
            var matrix1 = MatrixCalculator.InitializeMatrix(numberOfRows, numberOfColumns);
            var matrix2 = MatrixCalculator.InitializeMatrix(numberOfRows, numberOfColumns);
            var result = MatrixCalculator.InitializeMatrix(numberOfRows, numberOfColumns);

            var coordinates = MatrixCalculator.GetCoordinatesOfMatrix(numberOfRows, numberOfColumns);

            List<Thread> threads = new();
            int threadCount = 4;
            int batchSize = coordinates.Count / threadCount;

            List<List<List<int>>> batches = new();

            for (int i = 0; i < coordinates.Count - batchSize; i+= batchSize)
            {
                batches.Add(coordinates.GetRange(i, batchSize));
            }

            int remaining = coordinates.Count % threadCount;
            for (int i = 0; i < remaining; i++)
            {
                var remain = coordinates[coordinates.Count - i - 1];
                batches[i].Add(remain);
            }

            //   1.Create an actual thread for each task (use the low - level thread mechanism from the programming language);
            // batches.ForEach(batch => {
            //     threads.Add(new Thread(()=> {
            //         MatrixCalculator.Compute(matrix1, matrix2, batch, numberOfColumns, result);
            //     }));
            // });
            //
            // threads.ForEach(thread => thread.Start());
            // threads.ForEach(thread => thread.Join());
            //
            // result.ForEach(row =>
            // {
            //     row.ForEach(element => Console.Write(element + " "));
            //     Console.WriteLine();
            // });



            // 2.Use a thread pool
            coordinates.ForEach(coordinate =>
            {
                ThreadPool.QueueUserWorkItem(_ =>
                    MatrixCalculator.ComputeTask(matrix1, matrix2, coordinate, numberOfColumns, result));
            });

            // Initialize counter with 1, otherwise, the AddCount method will throw an error (the counter is signaled and cannot be incremented)-
            using (CountdownEvent countdownEvent = new CountdownEvent(1))
            {
                ManualResetEvent manualResetEvent = new ManualResetEvent(false);

                coordinates.ForEach(coordinate =>
                {
                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        // Increase the counter
                        countdownEvent.AddCount();
                        // Execute the business logic: a thread-safe withdraw books (implements lock internally)
                        MatrixCalculator.ComputeTask(matrix1, matrix2, coordinate, numberOfColumns, result);
                        // Decrease the counter
                        countdownEvent.Signal();
                        // Signal that at least one thread is executed.
                        manualResetEvent.Set();
                    });
                });

                // ensure that at least one thread was created 
                manualResetEvent.WaitOne();

                // Decrease the counter (as it was initialized with the value 1).
                countdownEvent.Signal();

                // Wait until the counter is zero.
                countdownEvent.Wait();

            }

            result.ForEach(row =>
             {
                 row.ForEach(element => Console.Write(element + " "));
                 Console.WriteLine();
             });
        }
    }
}
