using System.Collections.Generic;

namespace Problema3.Domain
{
    public class InternalNode : BaseNode
    {
        public List<Node> ParentNodes { get; set; }

        public override int ComputeValue()
        {
            Value = 0;

            ParentNodes.ForEach(d => Value += d.ComputeValue());

            return Value;
        }

        public override bool IsConsistent()
        {
            int sum = 0;

            ParentNodes.ForEach(d => sum += d.ComputeValue());

            return sum == Value;
        }
    }
}
