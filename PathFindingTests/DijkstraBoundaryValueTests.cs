using PathFinding.src;

namespace PathFinding.Tests;

public class DijkstraBoundaryValueTests
{
    private Algoritms algo;
    private string _buffer;

    private void Setup(Cell start, Cell end)
    {
        _buffer = string.Empty;

            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        algo = new Algoritms(grid, start, end)
        {
            Action = txt => { _buffer += txt; }
        };
        algo.AlgoSearch(algo.Type["Dijkstra"]);
        algo.DrawResultingGraph();
    }

    [Test]
    public void Dijkstra_With_X_Of_Start_Node_Close_To_Up_Range()
    {
        var start = new Cell(0, 1);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . s . \n" +
                     "1 . e . \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_Start_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 0);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 4\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 s e . \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_Start_Node_Close_To_Down_Range()
    {
        var start = new Cell(2, 1);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 . e . \n" +
                     "2 . s X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_Start_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 2);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 . e s \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_End_Node_Close_To_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(0, 1);

        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . e . \n" +
                     "1 . s . \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_End_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 0);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 e s . \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_End_Node_Close_To_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(2, 1);
        
        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 . s . \n" +
                     "2 . e X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_End_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 2);

        var expect = "Number of iterations: 4\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 \n" +
                     "0 . X . \n" +
                     "1 . s e \n" +
                     "2 . . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_Start_Node_Close_Out_Up_Range()
    {
        var start = new Cell(-1, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_Start_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, -1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_Start_Node_Close_Out_Down_Range()
    {
        var start = new Cell(3, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_Start_Node_Close_Out_Right_Range()
    {
        var start = new Cell(1, 3);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_End_Node_Close_Out_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(-1, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_End_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, -1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_X_Of_End_Node_Close_Out_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(3, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }

    [Test]
    public void Check_Dijkstra_With_Y_Of_End_Node_Close_Out_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }
}