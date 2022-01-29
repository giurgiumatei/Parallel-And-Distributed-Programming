
static List<List<int>> GetSubsetsOfElements(List<int> superset, int k)
{
    for (var i = 0; i < superset.Count; i++)
    {
        var subset = new List<int>();
        int counter = 0;

        while (counter != k)
        {
            subset.Add(superset[i + counter]);
            counter++;  
        }   
    }
}
