using System.Security.Cryptography;

namespace CompareSearchPath.Models;

public class Grid
{
    public JsonStructure grid_params { get; set; }

    public bool[,] grid_matrix;

    public Grid(JsonStructure grid_params) 
    {
        this.grid_params = grid_params;

    }

    public bool[,] generate_grid() 
    {
        var grid_matrix = new bool[grid_params.grid_size];
        return grid_matrix;
    }

}