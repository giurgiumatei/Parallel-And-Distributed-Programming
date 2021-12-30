using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lab6.Domain;

namespace Lab6
{
    class Program
    {
        private const int NumberOfGraphs = 3;

        static void Main(string[] args)
        {
            for (int i = 1; i <= NumberOfGraphs; i++)
            {
                var graph = DirectedGraph.GenerateRandomHamiltonian(i * 100); // nr of vertices
                Test(i, graph);
            }
        }

        private static void Test(int vertices, DirectedGraph graph)
        {
            var stopWatch = Stopwatch.StartNew();

            Find(graph);
            stopWatch.Stop();

            Console.WriteLine($"{vertices * 10} vertices: {stopWatch.Elapsed.TotalMilliseconds} ms");
        }

        private static void Find(DirectedGraph graph)
        {
            var result = new List<int>(graph.Size());

            List<Task> tasks = new List<Task>();

            for (var i = 0; i < graph.Size(); i++)
            {
                var startingNode = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    new CycleFinder
                            {DirectedGraph = graph, StartingNode = startingNode, Result = result, Flag = false}
                        .Visit(startingNode);
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
