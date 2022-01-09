using PuzzleProblemParallel.Domain;
using System.Diagnostics;



var initialState = Matrix.FromFile();
var stopWatch = Stopwatch.StartNew();
var solution = Solve(initialState);

stopWatch.Stop();
Console.WriteLine(solution);
Console.WriteLine($"Run time: {stopWatch.Elapsed.TotalMilliseconds} ms");

static Matrix Solve(Matrix root)
{
    var stopWatch = Stopwatch.StartNew();
    var minimumBound = root.Manhattan;

    while (true)
    {
        var (minimumDistance, state) = SearchParallel(root, 0, minimumBound, 5);

        if (minimumDistance == -1)
        {
            stopWatch.Stop();
            Console.WriteLine($"Solution found in {state.NumberOfSteps} steps");
            Console.WriteLine($"Execution time was {stopWatch.Elapsed.TotalMilliseconds} ms");
            return state;
        }

        Console.WriteLine($"Depth + {minimumDistance} reached in {stopWatch.Elapsed.TotalMilliseconds} ms");

        minimumBound = minimumDistance;
    }
}

static KeyValuePair<int, Matrix> SearchParallel(Matrix current, int numberOfSteps, int bound, int numberOfTasks)
{
    if (numberOfTasks <= 1)
    {
        return Search(current, numberOfSteps, bound);
    }

    var estimation = numberOfSteps + current.Manhattan;

    if (estimation > bound)
    {
        return new KeyValuePair<int, Matrix>(estimation, current);
    }

    if (estimation > 80)
    {
        return new KeyValuePair<int, Matrix>(estimation, current);
    }

    if (current.Manhattan == 0)
    {
        return new KeyValuePair<int, Matrix>(-1, current);
    }

    var minimum = int.MaxValue;
    var moves = current.GenerateMoves();

    var tasks = moves
        .Select(next => new Task<KeyValuePair<int, Matrix>>(() => SearchParallel(next, numberOfSteps + 1, bound, numberOfTasks / moves.Count)))
        .ToList();

    foreach (var task in tasks)
    {
        task.Start();
    }

    foreach (var task in tasks)
    {
        var (candidate, state) = task.Result;

        if (candidate == -1)
        {
            return new KeyValuePair<int, Matrix>(-1, state);
        }

        if (candidate < minimum)
        {
            minimum = candidate;
        }
    }

    return new KeyValuePair<int, Matrix>(minimum, current);
}

static KeyValuePair<int, Matrix> Search(Matrix current, int numberOfSteps, int bound)
{
    var estimation = numberOfSteps + current.Manhattan;

    if (estimation > bound)
    {
        return new KeyValuePair<int, Matrix>(estimation, current);
    }

    if (estimation > 80)
    {
        return new KeyValuePair<int, Matrix>(estimation, current);
    }

    if (current.Manhattan == 0)
    {
        return new KeyValuePair<int, Matrix>(-1, current);
    }

    var minimum = int.MaxValue;
    Matrix solution = null;

    foreach (var next in current.GenerateMoves())
    {
        var (candidate, state) = Search(next, numberOfSteps + 1, bound);

        if (candidate == -1)
        {
            return new KeyValuePair<int, Matrix>(-1, state);
        }

        if (candidate >= minimum) continue;

        minimum = candidate;
        solution = state;
    }

    return new KeyValuePair<int, Matrix>(minimum, solution);
}
