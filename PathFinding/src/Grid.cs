using Spectre.Console;

namespace PathFinding.src;


public class Grid
{
    public JsonModel grid_params { get; set; }

    public bool[,] matrix { get; set; }
    public int[,] walls { get; set; }
    public int width { get; set; }
    public int height { get; set; }

    public Grid(JsonModel grid_params)
    {
        this.grid_params = grid_params;
        this.walls = grid_params.walls;
        this.width = grid_params.grid_size[0];
        this.height = grid_params.grid_size[1];
        generate_grid();
    }

    public Grid(int width, int height, int [,] walls)
    {
        this.width = width;
        this.height = height;
        this.walls = walls;
        this.matrix = new bool[width, height];
        for (int i = 0; i < walls.GetLength(0); i++)
        {
            this.matrix[walls[i, 0], walls[i, 1]] = true;
        }
    }

    public Grid()
    {
    }


    public bool[,] generate_grid()
    {
        this.matrix = new bool[width, height];

        // set walls
        for (int i = 0; i < walls.GetLength(0); i++)
        {
            this.matrix[walls[i,0],walls[i,1]] = true;
        }
        return this.matrix;
    }

    public static bool[,] static_generate_grid(int width, int height, int[,] walls)
    {
        bool[,] matrix = new bool[width, height];
        for (int i = 0; i < walls.GetLength(0); i++)
        {
            matrix[walls[i,0],walls[i,1]] = true;
        }
        return matrix;
    }

}