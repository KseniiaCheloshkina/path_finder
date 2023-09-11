namespace pathFinding.src;

public class JsonStructure
{
    public int[] grid_size { get; set; }
    public int[][] walls { get; set; }
    public int[] start_node { get; set; }
    public int[] end_node { get; set; }
}