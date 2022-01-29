namespace M_Coloring_Problem.Domain;

public class Node
{
    public int Index { get; set; }
    public int? Color { get; set; }
    public List<Node> Neighbors { get; set; } = new();
}