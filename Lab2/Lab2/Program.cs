using Lab2.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Buffer = Lab2.Domain.Buffer;

namespace Lab2
{
    class Program
    {
        public static int CorrectSum(List<int> firstVector, List<int> secondVector)
        {
            int sum = 0;

            for (int i = 0; i < firstVector.Count; i++)
            {
                sum += firstVector[i] * secondVector[i];
            }

            return sum;
        }

        public static void Main(string[] args)
        {
            Buffer buffer = new Buffer {Size = 1};
            var firstVector = Enumerable.Range(1, 1000).ToList();
            var secondVector = Enumerable.Range(1, 1000).ToList();
        
            int correctSum = CorrectSum(firstVector, secondVector);

            Producer producer = new Producer 
            {
                    Buffer = buffer, 
                    FirstVector = firstVector, 
                    SecondVector = secondVector, 
                    NumberOfTasks = 1000
            };
            Consumer consumer = new Consumer
            {
                Buffer = buffer,
                NumberOfTasks = 1000
            };

            ThreadStart firstThreadStart = delegate { producer.CalculateProduct(); };
            ThreadStart secondThreadStart = delegate { consumer.CalculateSum(); };

            Thread producerThread = new Thread(firstThreadStart);
            Thread consumerThread = new Thread(secondThreadStart);

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();

            Console.WriteLine($"Dot product computed by the threads is {consumer.Sum} and correct sum is {correctSum}");


        }
    }
}
