using System.Diagnostics;
using System.Security.Cryptography;

namespace pathFinding.src;

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
        grid_matrix = new bool[grid_params.grid_size[0], grid_params.grid_size[1]];

        // set walls
        foreach (var wall in grid_params.walls)
        {
            grid_matrix[wall[0], wall[1]] = true;
        }
        return grid_matrix;
    }

}