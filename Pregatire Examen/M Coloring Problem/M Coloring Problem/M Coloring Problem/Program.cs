using M_Coloring_Problem.Domain;

var fileName =
    "C:\\Users\\giurg\\Desktop\\PDP\\M Coloring Problem\\M Coloring Problem\\M Coloring Problem\\Neighbors.txt";
var graph = Graph.CreateGraph(fileName);
var result = await CanSolve(graph, 1, 4, new List<int>());

if (result)
{
    graph.Nodes.ForEach(node => Console.WriteLine($"Node {node.Index}->Color {node.Color}"));
}
else
{
    Console.WriteLine("No result could be found!");
}

static bool IsSafe(Node node, int color)
{
    return !node.Neighbors
        .Select(node => node.Color)
        .Contains(color);
}


static  Task<bool> CanSolve(Graph graph, int currentColor, int maximumNumbersOfColors, List<int> visited)
{
    if (currentColor == maximumNumbersOfColors) return Task.FromResult(false);

    if (graph.Nodes[^1].Color != null)
    {
        return Task.FromResult(true);
    }

    graph.Nodes.ForEach(node => 
    {
        if (IsSafe(node, currentColor))
        {
            if (visited.Contains(node.Index))
            {
                return;
            }
            node.Color = currentColor;
            graph.Nodes.Select(n => n.Neighbors).ToList().ForEach(neighbors => neighbors.ForEach(neighbor =>
            {
                if (neighbor.Index == node.Index)
                {
                    neighbor.Color = currentColor;
                }
            }));
            visited.Add(node.Index);
        }
        else
        {
           Task.Run(() => CanSolve(graph, currentColor + 1, maximumNumbersOfColors, visited)).Wait();
        }
    });

    return Task.FromResult(true);
}