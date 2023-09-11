namespace CompareSearchPath.Models;

public class JsonModel
{
    public int N { get; set; }
    
    public int M { get; set; }
    
    public bool[,] Map { get; set; }
    
    public Node Start { get; set; }
    
    public Node End { get; set; }
}