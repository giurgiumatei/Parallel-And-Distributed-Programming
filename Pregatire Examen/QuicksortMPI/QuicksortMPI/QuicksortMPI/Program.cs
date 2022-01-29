using MPI;
using Environment = MPI.Environment;

var array = new List<int> {1, 10, 23, 9, 3, 75, 2};
var resultingList = new List<int>();

using (new Environment(ref args))
{
    var communicator = Communicator.world;
    var chunks = array.Chunk(4).ToList();


    for (int i = 0; i < chunks.Count; i++)
    {
        var values = chunks[i].ToList();

        if (communicator.Rank == 0)
        {
            communicator.Broadcast(ref values, 0);
            var result = communicator.Receive<List<int>>(Communicator.anySource, 1);
            resultingList.AddRange(result);
        }
        else
        {
            communicator.Broadcast(ref values, 0);
            values = QuickSort(values, 0, values.Count - 1);
            communicator.Send(values, 0, 1);
        }
    }

   // resultingList = QuickSort(resultingList, 0, resultingList.Count - 1);

    foreach (var i in resultingList)
    {
        Console.WriteLine(i);
    }
}

static List<int> QuickSort(List<int> array, int lowIndex, int highIndex)
{
    if (lowIndex >= highIndex)
    {
        return null;
    }

    var pivot = array[highIndex];
    var leftPointer = lowIndex;
    var rightPointer = highIndex;

    while (leftPointer < rightPointer)
    {
        while (array[leftPointer] <= pivot && leftPointer < rightPointer)
        {
            leftPointer++;
        }


        while (array[rightPointer] >= pivot && leftPointer < rightPointer)
        {
            rightPointer--;
        }

        Swap(array, leftPointer, rightPointer);

    }

    Swap(array, leftPointer, highIndex);
    QuickSort(array, lowIndex, leftPointer - 1);
    QuickSort(array, leftPointer + 1, highIndex);

    return array;
}


static void Swap(List<int> array, int leftPointer, int rightPointer)
{
    (array[leftPointer], array[rightPointer]) = (array[rightPointer], array[leftPointer]);
}