using Problema3.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Problema3
{
    public class ComputationalGraph
    {
        public Mutex GraphMutex { get; set; } = new();
        public List<Node> Nodes { get; set; } = new();
        public int NumberOfBaseVariables { get; set; }
        public int NumberOfSecondaryVariables { get; set; }

        public static void DfsForAddingToItsChildren(Node node, int quantity)
        {
            node.AddToValue(quantity);

            ((BaseNode)node).ChildNodes.ForEach(n => DfsForAddingToItsChildren(n, quantity));
        }

        public void UpdateBaseNode(BaseNode baseNode, int newValue)
        {
            GraphMutex.WaitOne();
            Console.WriteLine("Locking graph");

            DfsForAddingToItsChildren(baseNode, newValue - baseNode.ComputeValue());

            GraphMutex.ReleaseMutex();
            Console.WriteLine("Unlocking graph");
        }

        public void ComputeInitialValue()
        {
            Nodes.ForEach(node => node.ComputeValue());
        }

        public void CheckConsistency()
        {
            Console.WriteLine("Checking consistency...");
            Nodes.ForEach(node =>
            {
                _ = node.IsConsistent() ? true : throw new Exception();
            });
            Console.WriteLine("Consistent!");
        }

        public void PrintValues()
        {
            Nodes.ForEach(node => Console.WriteLine($"Value of {node.IndexInTheList} is {node.Value}"));
        }

        public BaseNode GetRandomBaseNode()
        {
            Random random = new Random();
            return (BaseNode)Nodes[random.Next(0, NumberOfBaseVariables)];
        }

        public void LoadGraph(string FileName)
        {
            var reader = new StreamReader(File.OpenRead(FileName));

            NumberOfBaseVariables = int.Parse(reader.ReadLine());

            for (int i = 0; i < NumberOfBaseVariables; i++)
            {
                int valueOfBaseNode = int.Parse(reader.ReadLine());
                var node = new BaseNode
                {
                    IndexInTheList = i,
                    Value = valueOfBaseNode,
                    ChildNodes = new List<Node>()
                };
                Nodes.Add(node);
            }

            NumberOfSecondaryVariables = int.Parse(reader.ReadLine());

            for (int i = 0; i < NumberOfSecondaryVariables; i++)
            {
                var node = new InternalNode
                {
                    IndexInTheList = Nodes.LastOrDefault().IndexInTheList + 1,
                    ChildNodes = new List<Node>()
                };
                int numberOfParents = int.Parse(reader.ReadLine());
                var parentNodes = new List<Node>();

                while(numberOfParents != 0)
                {
                    int indexOfParent = int.Parse(reader.ReadLine());
                    parentNodes.Add(Nodes[indexOfParent]);
                    ((BaseNode)Nodes[indexOfParent]).ChildNodes.Add(node);
                    numberOfParents--;
                }

                node.ParentNodes = parentNodes;
                node.Value = node.ComputeValue();
                Nodes.Add(node);
            }
        }
    }
}
