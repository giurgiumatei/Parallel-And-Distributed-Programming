using ComputeSumMatrixBinaryTree;

var tasks = new List<Task<int?>>();
var matrix = ConstructMatrix(4);

var binaryTree = new Tree();
binaryTree.Root = binaryTree.InsertLevelOrder(ValuesFromMatrix(matrix), binaryTree.Root, 0);
GetTasks(binaryTree.Root, tasks);
var sum = binaryTree.Root.Value;

Task.WaitAll(tasks.ToArray());
Parallel.ForEach(tasks, task => Interlocked.Add(ref sum, task.Result ?? 0));
Console.WriteLine(sum);

static void GetTasks(Node? node, List<Task<int?>> tasks)
{
    if (node == null)
    {
        return;
    }

    tasks.Add(Task.Run(() => node.Left?.Value + node.Right?.Value));

    GetTasks(node.Left, tasks);
    GetTasks(node.Right, tasks);
}

static List<int> ValuesFromMatrix(int[][] matrix)
{
    var valuesFromMatrix = new List<int>();

    for (var i = 0; i < matrix.Length; i++)
    {
        for (var j = 0; j < matrix[i].Length; j++)
        {
            valuesFromMatrix.Add(matrix[i][j]);
        }
    }

    return valuesFromMatrix;
}


static int[][] ConstructMatrix(int numberOfColumns)
{
    var matrix = new int[numberOfColumns][];
    for (int i = 0; i < matrix.Length; i++)
    {
        matrix[i] = new int[numberOfColumns];
    }

    var count = 1;

    for (var i = 0; i < matrix.Length; i++)
    {
        for (var j = 0; j < matrix[i].Length; j++)
        {
            matrix[i][j] = count;
            count++;
        }
    }

    return matrix;
}