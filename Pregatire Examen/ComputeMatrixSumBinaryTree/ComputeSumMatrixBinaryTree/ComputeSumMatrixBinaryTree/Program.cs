using ComputeSumMatrixBinaryTree;

var matrix = ConstructMatrix(4);

var binaryTree = ConstructBinaryTree(matrix);

Console.WriteLine(CalculateSum(binaryTree[0]));

static int CalculateSum(Node node)
{
    if (node == null)
    {
        return 0;
    }
    return CalculateSum(node.Left) + CalculateSum(node.Right) + node.Value;
}

static List<Node> ConstructBinaryTree(int[][] matrix)
{
    var valuesFromMatrix = new List<int>();

    for (var i = 0; i < matrix.Length; i++)
    {
        for (var j = 0; j < matrix[i].Length; j++)
        {
            valuesFromMatrix.Add(matrix[i][j]);
        }
    }


    var nodes = new List<Node>();

    for (int i = 0; i < valuesFromMatrix.Count; i++)
    {
        var leftChild = new Node();
        var rightChild = new Node();
        var currentNode = new Node();

        if (i == 0)
        {
            leftChild.Value = valuesFromMatrix[i + 1];

            rightChild.Value = valuesFromMatrix[i + 2];

            currentNode.Value = valuesFromMatrix[i];
            currentNode.Left = leftChild;
            currentNode.Right = rightChild;

            i += 2;
        }
        else if (i < valuesFromMatrix.Count)
        {
            if (i + 1 < valuesFromMatrix.Count)
            {
                leftChild.Value = valuesFromMatrix[i + 1];
            }

            if (i + 2 < valuesFromMatrix.Count)
            {
                rightChild.Value = valuesFromMatrix[i + 2];
            }


            currentNode.Value = valuesFromMatrix[i];
            currentNode.Left = leftChild;
            currentNode.Right = rightChild;

            if (i % 2 == 1)
            {
                nodes[nodes.Count - 1].Left = currentNode;
            }
            else
            {

                if (i % 2 == 1)
                {
                    nodes[nodes.Count - 1].Right = currentNode;
                }
            }

            i += 2;
        }
        nodes.Add(currentNode);
    }

    return nodes;
}


static int[][] ConstructMatrix(int numberOfColumns)
{
    var matrix = new int[numberOfColumns][];
    for (int i = 0; i < matrix.Length; i++)
    {
        matrix[i] = new int[numberOfColumns];
    }

    int count = 1;

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




