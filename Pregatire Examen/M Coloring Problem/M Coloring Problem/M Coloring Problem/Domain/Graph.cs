namespace M_Coloring_Problem.Domain;

public class Graph
{
    public List<Node> Nodes { get; set; }

    public static Graph CreateGraph(string fileName)
    {
        var nodes = File.ReadAllLines(fileName)
            .Select((line, index) =>
            {
                var neighbors = line.Split(' ')
                    .Select(int.Parse)
                    .Select(i => new Node {Index = i})
                    .ToList();

                return new Node
                {
                    Neighbors = neighbors,
                    Index = index
                };
            })
            .ToList();

        return new Graph
        {
            Nodes = nodes
        };
    }
}