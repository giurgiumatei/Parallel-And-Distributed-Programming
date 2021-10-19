using System.Threading;

namespace Problema3.Domain
{
    public abstract class Node
    {
        public Mutex Mutex { get; set; } = new();
        public int Value { get ; set; }
        public int IndexInTheList { get; set; }

        public abstract void AddToValue(int valueToBeAdded);
        public abstract int ComputeValue();
        public abstract bool IsConsistent();
    }
}
