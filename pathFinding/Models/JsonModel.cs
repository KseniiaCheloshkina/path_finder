namespace CompareSearchPath.Models;

public class JsonStructure
{
    public int[,] grid_size { get; set; }
    public int[,] walls { get; set; }
    public int[,] start_node { get; set; }
    public int[,] end_node { get; set; }

    //public int N { get; set; }
    
    //public int M { get; set; }
    
    //public bool[,] Map { get; set; }
    
    //public Node Start { get; set; }
    
    //public Node End { get; set; }
}