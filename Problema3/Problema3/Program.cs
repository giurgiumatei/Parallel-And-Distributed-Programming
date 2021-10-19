using Problema3.Domain;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Problema3
{
    class Program
    {
        static void Main(string[] args)
        {
            int T = 1000;
            var start = DateTime.Now;
            List<Thread> threads = new List<Thread>();
            ComputationalGraph g = new ComputationalGraph();
            Random random = new Random();


            g.LoadGraph("C:\\Facultate\\Anul 3\\PDP\\Lab1 Refacut\\Problema3\\Problema3\\graph_small.in");
            g.ComputeInitialValue();

            for (int t = 0; t < T; ++t)
            {
                if (t % 5 == 0)
                {
                    // check for consistency
                    for (int i = 0; i < threads.Count; ++i)
                    {
                        threads[i].Join();
                    }

                    threads.Clear();
                    g.CheckConsistency();
                }

                BaseNode currentNode = g.GetRandomBaseNode();
                int newVal = random.Next(-10, 11);

                Console.WriteLine($"Thread {t + 1} updates node {currentNode.IndexInTheList} with {newVal} \n");
                ThreadStart threadStart = delegate { g.UpdateBaseNode(currentNode, newVal); };
                Thread thread = new Thread(threadStart);
                thread.Start();
                threads.Add(thread);

            }

            for (int t = 0; t < threads.Count; ++t)
            {
                threads[t].Join();
            }

            threads.Clear();
            g.CheckConsistency();
            g.PrintValues();
            var end = DateTime.Now;
            var elapsedTime = end - start;

            Console.WriteLine($"\nElapsed time: {elapsedTime.TotalSeconds}");

        }
    }
}
