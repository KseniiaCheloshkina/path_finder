﻿using PathFinding.src;

namespace PathFinding.Tests;

public class DijkstraTests
{
    private Algoritms algo;
    private string result;
    
    private void Setup(Grid grid, Cell start, Cell end)
    {
        result = string.Empty;
        algo = new Algoritms(grid, start, end)
        {
            Action = txt => { result += txt; }
        };
        algo.AlgoSearch(algo.Type["Dijkstra"]);
        algo.DrawResultingGraph();
    }

    [Test]
    public void Correct_Map_And_Start_And_End_Path_Found()
    {   
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 3\n" +
                     "Path weight: 10\n" +
                     "  0 1 2 \n" +
                     "0 s X . \n" +
                     "1 * X . \n" +
                     "2 e . X \n" +
                     "s - start\n" +
                     "f - finish\n";
        // act
        Setup(grid, start, end);
        
        // assert
        Assert.AreEqual(expect, result);
    }
    
    [Test]
    public void Correct_Map_And_Start_And_End_Path_Not_Found()
    {
            // { false, false, false },
            // { true, true, true },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{1,2},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        var expect = "Number of iterations: 3\n" +
                     "Number of cells: 3\n" +
                     "Path weight: 10\n" +
                     "  0 1 2 \n" +
                     "0 s X . \n" +
                     "1 * X X \n" +
                     "2 e . X \n" +
                     "s - start\n" +
                     "f - finish\n";

        Setup(grid, start, end);
        
        Assert.AreEqual(expect, result);
    }
    
    [Test]
    public void Correct_Map_And_Start_Neighbouring_End()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(0, 1);
        
        Setup(grid, start, end);
    }
    
    [Test]
    public void Correct_Map_And_Start_Equal_End()
    {
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(0, 0);
        
        var exception = Assert.Throws<Exception>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Start and end positions are the same", exception.Message);
    }
    
    [Test]
    public void Map_Is_Empty()
    {
        var grid = new Grid();
        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        
        var exception = Assert.Throws<System.NullReferenceException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Object reference not set to an instance of an object.", exception.Message);
    }
    
    [Test]
    public void Correct_Map_And_Incorrect_Start()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 3);
        var end = new Cell(2, 0);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }
    
    [Test]
    public void Correct_Map_And_Incorrect_End()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Index was outside the bounds of the array.", exception.Message);
    }
}