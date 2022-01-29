var resultList = new List<List<int>>();
var numbers = Enumerable.Range(1, 5).ToArray();
var threads = new List<Thread>();

Permute(numbers, resultList,threads);
threads.ForEach(thread => thread.Start());
threads.ForEach(thread => thread.Join());

Console.WriteLine(resultList.Count);


void Permute(int[] numbers, List<List<int>> resultList, List<Thread> threads)
{
    DoPermute(numbers, 0, numbers.Length - 1, resultList, threads);
}

 void DoPermute(int[] numbers, int start, int end, List<List<int>> list, List<Thread> threads)
 {
     if (start == end)
    {
        threads.Add(new Thread(() => { Condition(numbers, list);}));
    }
    else
    {
        for (var i = start; i <= end; i++)
        {
            Swap(ref numbers[start], ref numbers[i]);
            DoPermute(numbers, start + 1, end, list, threads);
            Swap(ref numbers[start], ref numbers[i]);
        }
    }
 }

static void Swap(ref int a, ref int b)
{
    (a, b) = (b, a);
}

static void Condition(int[] numbers, List<List<int>> resultList)
{
    if (Pred(numbers))
    {
        resultList.Add(new List<int>(numbers));
    }
}

static bool Pred(int[] numbers) => numbers.SequenceEqual(Enumerable.Range(1, 5).ToList());