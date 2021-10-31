using System;

namespace Lab2.Domain
{
    public class Consumer
    {
        public Buffer Buffer { get; set; }
        public int Sum { get; set; } = 0;
        public int NumberOfTasks { get; set; }

        public void CalculateSum()
        {

            for (int i = 0; i < NumberOfTasks; i++)
            {
                Sum += Buffer.Pop();
            }

            Console.WriteLine("Consumer Done!");
        }
    }
}
