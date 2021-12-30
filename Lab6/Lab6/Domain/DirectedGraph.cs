using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab6.Extensions;

namespace Lab6.Domain
{
    public class DirectedGraph
    {
        public List<List<int>> AdjacencyMatrix { get; set; }
        public List<int> Nodes { get; set; }

        public DirectedGraph(int nodeCount)
        {
            AdjacencyMatrix = new List<List<int>>();
            Nodes = new List<int>();

            for (var i = 0; i < nodeCount; i++)
            {
                AdjacencyMatrix.Add(new List<int>());
                Nodes.Add(i);
            }
        }

        public static DirectedGraph GenerateRandomHamiltonian(int size)
        {
            var directedGraph = new DirectedGraph(size);
            var nodes = directedGraph.Nodes.Shuffle();

            for (var i = 1; i < nodes.Count; i++)
            {
                directedGraph.AddEdge(nodes[i - 1], nodes[i]);
            }

            directedGraph.AddEdge(nodes[^1], nodes[0]);

            var random = new Random();

            for (var i = 0; i < size / 2; i++)
            {
                var nodeA = random.Next(size - 1);
                var nodeB = random.Next(size - 1);

                directedGraph.AddEdge(nodeA, nodeB);
            }

            return directedGraph;
        }

        public void AddEdge(int firstNode, int secondNode) => AdjacencyMatrix[firstNode].Add(secondNode);

        public List<int> Neighbors(int node) => AdjacencyMatrix[node];

        public int Size() => AdjacencyMatrix.Count;
    }
}
