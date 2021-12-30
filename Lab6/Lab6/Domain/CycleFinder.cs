using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.Domain
{
    public class CycleFinder
    {
        public DirectedGraph DirectedGraph { get; set; }
        public int StartingNode { get; set; }
        public List<int> Path { get; set; } = new();
        public List<int> Result { get; set; }
        public bool Flag { get; set; }

        private static readonly object Lock = new();

        public void Visit(int node)
        {
            Path.Add(node);

            if (!Flag)
            {
                if (Path.Count == DirectedGraph.Size())
                {
                    if (DirectedGraph.Neighbors(node).Contains(StartingNode))
                    {
                        //a cycle was found
                        Flag = true;
                        lock (Lock)
                        {
                            Result.Clear();
                            Result.AddRange(Path);
                        }
                    }
                    return;
                }

                DirectedGraph.Neighbors(node).ForEach(neighbor =>
                {
                    if (!Path.Contains(neighbor))
                    {
                        Visit(neighbor);
                    }
                });
            }
        }
    }
}
