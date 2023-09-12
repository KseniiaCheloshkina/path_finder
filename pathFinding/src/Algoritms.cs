namespace pathFinding.src;


public class Algoritms
{
    private readonly bool[,] _grid;
    private int _iter = 0;
    private readonly Cell _start_loc;
    private readonly Cell _end_loc;
    private Cell? _path;

    public Action<string>? Action;

    public Cell? Path => _path;

    public bool[,] GridMatrix => _grid;
    public int width => _grid.GetLength(0);
    public int height => _grid.GetLength(1);

    public Cell StartPos => _start_loc;
    public Cell EndPos => _end_loc;
    public bool EmptyFlag => _grid.GetLength(0) == 0 || _grid.GetLength(1) == 0;

    public Dictionary<string, string> Type = new Dictionary<string, string>()
    {
        { "Dijkstra", "distance_from_start"},
        { "AStar", "final_distance"}
    };

    public Algoritms()
    {
        _grid = new bool[,] { };
        _start_loc = new Cell();
        _end_loc = new Cell();
        _path = null;
        Action = Console.Write;
        _iter = 0;
    }

    public Algoritms(bool[,] grid, Cell start_pos, Cell end_pos)
    {
       // Grid validation
        if (grid.GetLength(0) == 0 || grid.GetLength(1) == 0)
            throw new IndexOutOfRangeException("Grid should not be empty");

        if (start_pos.X == end_pos.X && start_pos.Y == end_pos.Y)
            throw new Exception("Start and end positions are the same");

        var out_of_range_condition_start = (0 <= start_pos.X && start_pos.X <= grid.GetLength(0)) && (0 <= start_pos.Y && start_pos.Y <= grid.GetLength(1));
        var out_of_range_condition_end = (0 <= end_pos.X && end_pos.X <= grid.GetLength(0)) && (0 <= end_pos.Y && end_pos.Y <= grid.GetLength(1));

        if (!out_of_range_condition_start || !out_of_range_condition_end)
            throw new IndexOutOfRangeException("Start and end positions should be inside a grid");

        // create a copy of grid
        _grid = new bool[grid.GetLength(0), grid.GetLength(1)];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                _grid[i, j] = grid[i, j];
            }
        }
        _start_loc = start_pos;
        _end_loc = end_pos;

        _iter = 0;

        _grid[start_pos.X, start_pos.Y] = false;
        _grid[end_pos.X, end_pos.Y] = false;

        _path = null;
        Action = Console.Write;
    }

    public void ResetAction() => Action = Console.Write;


    public void AlgoSearch(string distance_type)
    {
        BuildTree(distance_type);
        WriteSolving();

    }

    public Cell min_distance_cell(List<Cell> list_cells, string distance_type)
   {
        int min_val = int.MaxValue;
        var final_cell = new Cell();
        
        foreach (var cur_cell in list_cells)
        {
            // получаем из ячейки нужный вид расстояния в зависимости от алгоритма
            object dist_obj = cur_cell.GetType().GetProperty(distance_type).GetValue(cur_cell, null);
            int dist_val = Convert.ToInt32(dist_obj); // конвертация в число
            if (dist_val < min_val){
                min_val = dist_val;
                final_cell = cur_cell;
            }
        }
        return final_cell;
   }

    private void BuildTree(string distance_type)
    {
        _iter = 1;
        // set of cells to go to
        var open_set = new List<Cell> { _start_loc };
        // set of already passed cells
        var closed_set = new List<Cell>();

        // iterate through all cells
        while (open_set.Count > 0)
        {
            // get cell with min cum distance
            Cell curNode = min_distance_cell(open_set, distance_type);

            // if we already found it
            if (curNode.X == _end_loc.X && curNode.Y == _end_loc.Y)
            {
                _path = curNode;
                return;
            }

            open_set.Remove(curNode);
            closed_set.Add(curNode);

            // for each neighbor calculate distance and add to open set
            foreach (var neighbour in GetNeighbours(curNode))
            {
                // if cell is in closed set
                if (closed_set.Any(item => item.X == neighbour.X && item.Y == neighbour.Y))
                    continue;

                // select neighbour from open set or default val
                var openNode = open_set.FirstOrDefault(item => item.X == neighbour.X && item.Y == neighbour.Y);
                // if neighbour is not in open set, add it
                if (openNode == null)
                {
                    // calc euclidean distance
                    neighbour.distance_to_target = (Math.Sqrt(Math.Pow(_end_loc.X - neighbour.X, 2) + Math.Pow(_end_loc.Y - neighbour.Y, 2))) * 5.0;
                    open_set.Add(neighbour);
                }
                else
                {   // if current path to neighbor is more optimal than previos, replace it
                    if (neighbour.distance_from_start < openNode.distance_from_start)
                    {
                        openNode.Predecessor = curNode;
                        openNode.distance_from_start = neighbour.distance_from_start;
                    }
                }
            }
            _iter++;
        }
        _path = null;
    }

    // вспомогательный метод для нахождения пути
    private List<Cell> GetNeighbours(Cell cur)
    {
        var rows = _grid.GetLength(0);
        var cols = _grid.GetLength(1);

        var nodes = new List<Cell>
        {
            new Cell(cur.X - 1, cur.Y - 1, cur),
            new Cell(cur.X, cur.Y - 1, cur),
            new Cell(cur.X + 1, cur.Y - 1, cur),
            new Cell(cur.X - 1, cur.Y, cur),
            new Cell(cur.X + 1, cur.Y, cur),
            new Cell(cur.X - 1, cur.Y + 1, cur),
            new Cell(cur.X, cur.Y + 1, cur),
            new Cell(cur.X + 1, cur.Y + 1, cur)
        };

        for (var i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].X < 0 || nodes[i].X >= rows || nodes[i].Y < 0 || nodes[i].Y >= cols ||
                _grid[nodes[i].X, nodes[i].Y])
                nodes.RemoveAt(i--);
        }

        return nodes;
    }

    private void WriteSolving()
    {
        if (Action == null) return;

        var listNode = new List<Cell>();

        if (_path == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Action?.Invoke("Путь не найден!\n");
            Console.ResetColor();
        }
        else
        {
            Action?.Invoke($"Количество итераций: {_iter}\n" + $"Число ячеек: {_path.num_predecessors + 1}\n");
            Action?.Invoke($"Вес пути: {_path.final_distance}\n");

            _path = _path.Predecessor;

            while (_path.X != _start_loc.X || _path.Y != _start_loc.Y)
            {
                listNode.Add(_path);
                _path = _path.Predecessor;
            }
        }

        var maxLenCol = _grid.GetLength(1).ToString().Length + 1;
        var maxLenRow = _grid.GetLength(0).ToString().Length + 1;

        if (maxLenCol == 1)
            maxLenCol++;
        if (maxLenRow == 1)
            maxLenRow++;

        Action?.Invoke(" ".PadRight(maxLenRow));
        Console.ForegroundColor = ConsoleColor.Blue;
        for (int i = 0; i < _grid.GetLength(1); i++)
        {
            Action?.Invoke($"{i}".PadRight(maxLenCol));
        }
        Console.ResetColor();
        Action?.Invoke("\n");
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Action?.Invoke($"{i}".PadRight(maxLenRow));
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                Console.ResetColor();
                var cell = listNode.FirstOrDefault(x => x.X == i && x.Y == j);
                if (i == _start_loc.X && j == _start_loc.Y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Action?.Invoke("s".PadRight(maxLenCol));
                    continue;
                }
                if (i == _end_loc.X && j == _end_loc.Y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Action?.Invoke("e".PadRight(maxLenCol));
                    continue;
                }
                if (cell != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Action?.Invoke("*".PadRight(maxLenCol));
                    listNode.Remove(cell);
                    continue;
                }
                Action?.Invoke(_grid[i, j] ? "[]".PadRight(maxLenCol) : "0".PadRight(maxLenCol));
            }
            Action?.Invoke("\n");
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Action?.Invoke("s - start\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Action?.Invoke("e - end\n");
        Console.ResetColor();
    }

}