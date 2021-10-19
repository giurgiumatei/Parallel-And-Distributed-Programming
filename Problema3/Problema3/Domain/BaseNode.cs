using System.Collections.Generic;

namespace Problema3.Domain
{
    public class BaseNode : Node
    {
        public List<Node> ChildNodes { get; set; }

        public override void AddToValue(int valueToBeAdded)
        {
            Mutex.WaitOne();
            //Console.WriteLine("Locking " + Index);

            Value += valueToBeAdded;

            //   Console.WriteLine("Unlocking " + Index);
            Mutex.ReleaseMutex();
        }

        public override int ComputeValue()
        {
            Mutex.WaitOne();
            //  Console.WriteLine("Locking " + Index);

            int auxiliaryValue = Value;

            //   Console.WriteLine("Unlocking " + Index);
            Mutex.ReleaseMutex();

            return auxiliaryValue;
        }

        public override bool IsConsistent()
        {
            return true;
        }
    }
}
