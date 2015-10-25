using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MazeBuilder : MonoBehaviour {

    public int size;
    public int seed;
    public float percentOfExtraLinks = 20;
    public GameObject mazeBlock;
    int counter = 0;
    internal int[,] maze;

    void Awake () {
        if (size > 299)
            size = 299;
        else if (size < 10)
            size = 10;
        Generate();
        AddLinks();
        GenerateExtraLinks();
        Build();
    }

    void Build()
    {
        if (maze == null)
            return;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i, j] != 0)
                {
                    GameObject block = Instantiate(mazeBlock) as GameObject;
                    block.name = "block_(" + i + "," + j + ")";
                    block.transform.forward = Vector3.forward;
                    block.transform.parent = transform;
                    if (i > 0 && maze[i - 1, j] != 0)
                    {
                        DestroyImmediate(block.transform.FindChild("LeftWall").gameObject);
                    }
                    if (j > 0 && maze[i, j - 1] != 0)
                    {
                        DestroyImmediate(block.transform.FindChild("BackWall").gameObject);
                    }
                    if (i < size - 1 && maze[i + 1, j] != 0)
                    {
                        DestroyImmediate(block.transform.FindChild("RightWall").gameObject);
                    }
                    if (j < size - 1 && maze[i, j + 1] != 0)
                    {
                        DestroyImmediate(block.transform.FindChild("FrontWall").gameObject);
                    }
                    block.transform.position = new Vector3(i * 5, 0, j * 5);
                }
            }
        }
    }

    void Generate()
    {
        maze = new int[size, size];
        UnityEngine.Random.seed = seed;
        Point start = new Point(UnityEngine.Random.Range(0,size/2), UnityEngine.Random.Range(0, size/2));
        GetNextNodeRecursiveFrom(start);
        int[,] newMaze = maze;
        maze = new int[size, size];
        for (int i = 0; i< size / 2; i++)
        {
            for(int j = 0;j< size / 2; j++)
            {
                maze[i*2, j*2] = newMaze[i, j];
            }
        }
    }
    
    void GenerateExtraLinks()
    {
        List<Point> voidPositions = new List<Point>();
        for(int i = 0; i< size; i++)
        {
            for(int j = 0; j< size; j++)
            {
                if (maze[i,j] == 0)
                {
                    voidPositions.Add(new Point(i,j));
                }
            }
        }

        for(int i = 0; i< (int)((float)voidPositions.Count*(percentOfExtraLinks/100.0f)); i++)
        {
            int indexOfTIme = UnityEngine.Random.Range(0, voidPositions.Count);
            maze[voidPositions[indexOfTIme].x, voidPositions[indexOfTIme].y] = -20;
        }
    }

    void AddLinks()
    {
        if (maze == null)
            return;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (maze[i, j] == 0)
                {
                    if ((i > 0 && i < size - 1) && (maze[i - 1, j] == maze[i + 1, j] + 1 || maze[i - 1, j] == maze[i + 1, j] - 1))
                    {
                        maze[i, j] = -10;
                    }
                    if ((j > 0 && j < size - 1) && (maze[i, j - 1] == maze[i, j + 1] + 1 || maze[i, j - 1] == maze[i, j + 1] - 1))
                    {
                        maze[i, j] = -10;
                    }
                }
            }
        }
    }

    void GetNextNodeRecursiveFrom(Point p)
    {
        maze[p.x, p.y] = ++counter;
        int movesLeft;
        do
        {
            List<Point> possibleMoves = new List<Point>();
            if (p.x > 0 && maze[p.x - 1, p.y] == 0)
            {
                possibleMoves.Add(new Point(p.x - 1, p.y));
            }
            if (p.y > 0 && maze[p.x, p.y - 1] == 0)
            {
                possibleMoves.Add(new Point(p.x, p.y - 1));
            }
            if (p.x < (size / 2) - 1 && maze[p.x + 1, p.y] == 0)
            {
                possibleMoves.Add(new Point(p.x + 1, p.y));
            }
            if (p.y < (size / 2) - 1 && maze[p.x, p.y + 1] == 0)
            {
                possibleMoves.Add(new Point(p.x, p.y + 1));
            }
            movesLeft = possibleMoves.Count;
            if (movesLeft > 0)
            {
                Point newPoint = possibleMoves[UnityEngine.Random.Range(0, movesLeft)];
                GetNextNodeRecursiveFrom(newPoint);
            }
        } while (movesLeft > 0);
        counter--;
    }
}

[System.Serializable]
public class Point
{
    internal int x = 0, y = 0;
    
    public Point()
    {
        x = 0;
        y = 0;
    }
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        Point objAsPoint = obj as Point;
        return (x == objAsPoint.x && y == objAsPoint.y);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
}
