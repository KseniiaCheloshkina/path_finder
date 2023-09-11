namespace pathFinding.src;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }

    // Перемещения
    public int distance_from_start { get; set; } // Расстояние от стартовой точки до текущей
    public int distance_to_target { get; set; } // Эвристика - расстояние от текущей точки до целевой
    public int final_distance => distance_from_start + distance_to_target;
    public int price_hor_vert = 5;
    public int price_diagonal = 7;

    public int num_predecessors;
    public Cell Predecessor;

    public Cell() { }
    public Cell(int[] position)
    {
        X = position[0];
        Y = position[1];

        distance_from_start = distance_to_target = num_predecessors = 0;

        Predecessor = null;
    }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;

        distance_from_start = distance_to_target = num_predecessors = 0;

        Predecessor = null;
    }

    public Cell(int x, int y, Cell parent)
    {
        X = x;
        Y = y;

        if (x == parent.X || y == parent.Y)
            // вверх-вниз, влево-вправо
            distance_from_start = parent.distance_from_start + price_hor_vert;
        else
            // по диагонали
            distance_from_start = parent.distance_from_start + price_diagonal;

        Predecessor = parent;
        num_predecessors = Predecessor.num_predecessors + 1;
    }
}