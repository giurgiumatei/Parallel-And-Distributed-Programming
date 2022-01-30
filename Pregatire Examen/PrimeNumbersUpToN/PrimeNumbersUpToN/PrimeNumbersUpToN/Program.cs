using MPI;
using Environment = MPI.Environment;

static void Master()
{
    var numbers = GenerateNumbers(50);
    numbers.ForEach(e => Console.Write(e + " "));
    Console.WriteLine();
    Communicator.world.Broadcast(ref numbers, 0);
}

 static void Worker()
{
    var received = new List<int>();
    Communicator.world.Broadcast(ref received, 0);
    var root = 2450 / (Communicator.world.Size - 1);
    var highBound = root * Communicator.world.Rank + 1;
    var lowBound = 51 + root * (Communicator.world.Rank - 1) + 1;
    if (Communicator.world.Rank != 1)
    {
        lowBound -= 51;
    }
    else
    {
        lowBound -= 1;
    }

    Console.WriteLine($"High Bound is : {highBound} | Lower Bound {lowBound}");
    Console.Write($"{Communicator.world.Rank} : received ");
    received.ForEach(e => Console.Write(e + " "));
    Console.WriteLine();
}

 static List<int> GenerateNumbers(int limit)
{
    var result = new List<int>();
    for (int j = 0; j < limit; j++)
    {
        if (IsPrime(j))
        {
            result.Add(j);
        }
    }

    return result;
}

 static bool IsPrime(int i)
{
    if (i is 0 or 1)
    {
        return false;
    }
    var limit = i / 2 + 1;
    for (var j = 2; j < limit; j++)
    {
        if (i % j == 0)
        {
            return false;
        }
    }

    return true;
}

using (new Environment(ref args))
{
    if (Communicator.world.Rank == 0)
    {
        Master();
    }
    else
    {
        Worker();
    }
}