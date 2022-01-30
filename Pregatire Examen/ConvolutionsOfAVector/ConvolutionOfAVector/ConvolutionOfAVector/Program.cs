var firstVector = Enumerable.Range(1, 10).ToList();
var secondVector = Enumerable.Range(1, 10).ToList();
var tasks = new List<Task>();
var result = 0;

Solve(firstVector, secondVector, tasks,  result);
Task.WaitAll(tasks.ToArray());

Console.WriteLine(result);

static void Solve(List<int> firstVector, List<int> secondVector, List<Task> tasks, int result)
{
    var firstVectorPartitions = firstVector.Chunk(5).ToList();
    var secondVectorPartitions = secondVector.Chunk(5).ToList();

    for (int i = 0; i < firstVectorPartitions.Count; i++)
    {
        var i1 = i;
        tasks.Add(Task.Factory.StartNew(() => ComputeConvolution(firstVectorPartitions[i1], secondVectorPartitions[secondVectorPartitions.Count - i1 - 1],  result)));
    }

}

static void ComputeConvolution(int[] firstVectorPartition, int[] secondVectorPartition,int result)
{
    for (int i = 0; i < firstVectorPartition.Length; i++)
    {
        Interlocked.Add(ref result,
            firstVectorPartition[i] * secondVectorPartition[secondVectorPartition.Length - i - 1]);
    }
}