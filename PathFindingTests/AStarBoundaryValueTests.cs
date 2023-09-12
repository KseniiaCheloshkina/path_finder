using System;
using PathFinding.src;

namespace PathFinding.Tests;

public class AStarBoundaryValueTests
{
    private Algoritms algo;
    private string result;

    private void Setup(Cell start, Cell end)
    {
        result = string.Empty;
        int width = 5;
        int height = 5;
        int[,] walls = {{ 0, 1 },{ 2, 4}};
        Grid grid = new Grid(width,height,walls);
        algo = new Algoritms(grid, start, end)
        {
            Action = txt => { result += txt; }
        };
        algo.AlgoSearch(algo.Type["AStar"]);
        algo.DrawResultingGraph();
    }

    [Test]
    public void AStar_With_X_Of_Start_Node_Close_To_Up_Range()
    {
        var start = new Cell(0, 1);
        var end = new Cell(1, 1);
        
        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . s . . . \n" +
                     "1 . e . . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 0);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 s e . . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_To_Down_Range()
    {
        var start = new Cell(2, 1);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 . e . . . \n" +
                     "2 . s . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 2);
        var end = new Cell(1, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 . e s . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(0, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . e . . . \n" +
                     "1 . s . . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 0);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 e s . . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(2, 1);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 . s . . . \n" +
                     "2 . e . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 2);

        var expect = "Number of iterations: 2\n" +
                     "Number of cells: 2\n" +
                     "Path weight: 5\n" +
                     "  0 1 2 3 4 \n" +
                     "0 . X . . . \n" +
                     "1 . s e . . \n" +
                     "2 . . . . X \n" +
                     "3 . . . . . \n" +
                     "4 . . . . . \n" +
                     "s - start\n" +
                     "f - finish\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Up_Range()
    {
        var start = new Cell(-1, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, -1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Down_Range()
    {
        var start = new Cell(6, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Right_Range()
    {
        var start = new Cell(6, 3);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(-1, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, -1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(6, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(6, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Start and end positions should be inside a grid", exception.Message);
    }
}