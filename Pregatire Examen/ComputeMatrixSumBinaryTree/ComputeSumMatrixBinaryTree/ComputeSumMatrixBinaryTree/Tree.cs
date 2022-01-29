namespace ComputeSumMatrixBinaryTree;

public class Tree
{
    public Node Root { get; set; }

    public Node InsertLevelOrder(List<int> arr, Node root, int i)
    {
        if (i >= arr.Count) return root;

        root = new Node { Value = arr[i] };

        root.Left = InsertLevelOrder(arr, root.Left, 2 * i + 1);

        root.Right = InsertLevelOrder(arr, root.Right, 2 * i + 2);
        return root;
    }
}