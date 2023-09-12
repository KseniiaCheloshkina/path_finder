using Spectre.Console;

namespace pathFinding.src;


public class Grid
{
    public JsonModel grid_params { get; set; }

    public bool[,] grid_matrix;

    public Grid(JsonModel grid_params)
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

    public static bool[,] static_generate_grid(int width, int height, int[,] walls)
    {
        bool[,] grid_matrix = new bool[width, height];
        for (int i = 0; i < walls.GetLength(0); i++)
        {
            grid_matrix[walls[i,0],walls[i,1]] = true;
        }
        return grid_matrix;
    }

}