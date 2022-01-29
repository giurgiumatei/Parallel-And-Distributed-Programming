
var numbers = Enumerable.Range(1, 5).ToArray();
var tasks = new List<Task>();

PrintResult(Permute(numbers, tasks));

static IList<IList<int>> Permute(int[] numbers, List<Task> tasks)
{
    var list = new List<IList<int>>();
    return DoPermute(numbers, 0, numbers.Length - 1, list, tasks);
}

static IList<IList<int>> DoPermute(int[] numbers, int start, int end, IList<IList<int>> list, List<Task> tasks)
{
    if (start == end)
    {
        // We have one of our possible n! solutions,
        // add it to the list.
        list.Add(new List<int>(numbers));
    }
    else
    {
        for (var i = start; i <= end; i++)
        {
            Swap(ref numbers[start], ref numbers[i]);
            DoPermute(numbers, start + 1, end, list, tasks);
            Swap(ref numbers[start], ref numbers[i]);
        }
    }

    return list;
}

static void Swap(ref int a, ref int b)
{
    (a, b) = (b, a);
}

static void PrintResult(IList<IList<int>> lists)
{
    Console.WriteLine("[");
    foreach (var list in lists)
    {
        Console.WriteLine($"    [{string.Join(',', list)}]");
    }
    Console.WriteLine("]");
}

static bool Pred(List<int> numbers) => numbers.SequenceEqual(Enumerable.Range(1, 5).ToList());