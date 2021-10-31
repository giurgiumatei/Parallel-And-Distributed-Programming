using System;
using System.Collections.Generic;

namespace Lab2.Domain
{
    public class Producer
    {
        public List<int> FirstVector { get; set; }
        public List<int> SecondVector { get; set; }
        public Buffer Buffer { get; set; }
        public int NumberOfTasks { get; set; }

        public void CalculateProduct()
        {
            for (int i = 0; i < NumberOfTasks; i++)
            {
                Buffer.Add(FirstVector[i] * SecondVector[i]);
            }

            Console.WriteLine("Producer Done!");
        }

    }
}
