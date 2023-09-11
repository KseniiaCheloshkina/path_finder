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
        if (grid.GetLength(0) == 0 || grid.GetLength(1) == 0)
            throw new IndexOutOfRangeException("Карта пуста");
        if (start_pos.X < 0 || start_pos.Y < 0 || start_pos.X >= grid.GetLength(0) || start_pos.Y >= grid.GetLength(1))
            throw new IndexOutOfRangeException("Стартовая точка лежит вне диапазонах карты");

        if (end_pos.X < 0 || end_pos.Y < 0 || end_pos.X >= grid.GetLength(0) || end_pos.Y >= grid.GetLength(1))
            throw new IndexOutOfRangeException("Конечная точка лежит вне диапазонах карты");

        if (start_pos.X == end_pos.X && start_pos.Y == end_pos.Y)
            throw new Exception("Стартовая и конечная точки совпадают");

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

    public void AStarAlgo()
    {
        FindPath(SortByF);
        WriteSolving();
    }

    public void DijkstraAlgo()
    {
        FindPath(SortByG);
        WriteSolving();
    }

    // основной путь нахождения пути
    private void FindPath(Func<Cell, int> selector)
    {
        _iter = 0;
        var open = new List<Cell> { _start_loc };
        var close = new List<Cell>();

        while (open.Count > 0)
        {
            _iter++;
            var curNode = open.MinBy(selector);

            if (curNode.X == _end_loc.X && curNode.Y == _end_loc.Y)
            {
                _path = curNode;
                return;
            }

            open.Remove(curNode);
            close.Add(curNode);

            foreach (var neighbour in GetNeighbours(curNode))
            {
                if (close.Any(item => item.X == neighbour.X && item.Y == neighbour.Y))
                    continue;

                var openNode = open.FirstOrDefault(item => item.X == neighbour.X && item.Y == neighbour.Y);

                if (openNode == null)
                {
                    neighbour.GetH = (Math.Abs(_end_loc.X - neighbour.X) + Math.Abs(_end_loc.Y - neighbour.Y)) * 10;
                    open.Add(neighbour);
                }
                else
                {
                    if (neighbour.GetG < openNode.GetG)
                    {
                        openNode.Parent = curNode;
                        openNode.GetG = neighbour.GetG;
                    }
                }
            }
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

    private static int SortByF(Cell node) => node.GetF;
    private static int SortByG(Cell node) => node.GetG;

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
            Action?.Invoke($"Количество итераций: {_iter}\n" + $"Число ячеек: {_path.CountParent + 1}\n");
            Action?.Invoke($"Вес пути: {_path.GetF}\n");

            _path = _path.Parent;

            while (_path.X != _start_loc.X || _path.Y != _start_loc.Y)
            {
                listNode.Add(_path);
                _path = _path.Parent;
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