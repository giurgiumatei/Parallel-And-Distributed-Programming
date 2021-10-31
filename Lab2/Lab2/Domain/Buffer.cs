using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lab2.Domain
{
    public class Buffer
    {
        private static readonly object Lock = new();
        public LinkedList<int> Stack { get; set; } = new();
        public int Size { get; set; }

        public void Add(int number)
        {
            lock (Lock)
            {
                if (Stack.Count == Size)
                {
                    Monitor.Wait(Lock);
                }

                Stack.AddFirst(number);
                Console.WriteLine($"Added {number} to the buffer!");
                Monitor.PulseAll(Lock);
            }
        }

        public int Pop()
        {
            lock (Lock)
            {
                if (Stack.Count == 0)
                {
                    Monitor.Wait(Lock);
                }

                int elementToBeRemoved = Stack.First();
                Stack.RemoveFirst();

                Console.WriteLine($"Removed {elementToBeRemoved} from the buffer!");
                Monitor.PulseAll(Lock);

                return elementToBeRemoved;
            }
        }


    }
}
