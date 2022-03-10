using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchVertex
{
    public SearchVertex()
    {

    }
    public SearchVertex(Vector2Int point, SearchVertex parent)
    {
        Point = point;
        Parent = parent;
    }

    public Vector2Int Point;
    public SearchVertex Parent;
    int Cost;
}
public class AbbyValidatedRoom : Room
{
    public bool _hasUpExit;
    public bool _hasDownExit;
    public bool _hasLeftExit;
    public bool _hasRightExit;

    public bool _hasUpLeftPath;
    public bool _hasUpRightPath;
    public bool _hasUpDownPath;
    public bool _hasRightDownPath;
    public bool _hasRightLeftPath;
    public bool _hasDownLeftPath;

    bool DoesListContainPoint(List<SearchVertex> list, Vector2Int point)
    {
        foreach (SearchVertex searchVertex in list)
        {
            if (searchVertex.Point == point)
                return true;
        }
        return false;
    }

    List<Vector2Int> GetPathDFS(int[,] indexGrid, Vector2Int startPoint, Vector2Int endPoint)
    {
        List<SearchVertex> openSet = new List<SearchVertex>();
        List<SearchVertex> closedSet = new List<SearchVertex>();
        List<Vector2Int> path = new List<Vector2Int>();
        if (IsPointNavigable(indexGrid, startPoint))
        {
            SearchVertex startVertex = new SearchVertex();
            startVertex.Point = startPoint;
            startVertex.Parent = null;
            openSet.Add(startVertex);
        }

        while (openSet.Count > 0)
        {
            int index = openSet.Count - 1;
            SearchVertex currentVertex = openSet[index];
            openSet.RemoveAt(index);

            if (currentVertex.Point == endPoint)
            {
                List<Vector2Int> retVal = new List<Vector2Int>();

                while (currentVertex != null)
                {
                    retVal.Add(currentVertex.Point);
                    currentVertex = currentVertex.Parent;
                }

                retVal.Reverse();

                return retVal;
            }


            //up neighbor
            Vector2Int upNeighbor = new Vector2Int(currentVertex.Point.x, currentVertex.Point.y + 1);
            // openSet.Exists(x=>x.Point==upNeighbor); //=> "for each value of x, treat it as x.Point and do this comparison
            if (DoesListContainPoint(openSet, upNeighbor) == false && DoesListContainPoint(closedSet, upNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, upNeighbor))
                {
                    SearchVertex upNeighborVertex = new SearchVertex(upNeighbor, currentVertex);
                    openSet.Add(upNeighborVertex);
                }
            }

            //down neighbor
            Vector2Int downNeighbor = new Vector2Int(currentVertex.Point.x, currentVertex.Point.y - 1);

            if (DoesListContainPoint(openSet, downNeighbor) == false && DoesListContainPoint(closedSet, downNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, downNeighbor))
                {
                    SearchVertex downNeighborVertex = new SearchVertex(downNeighbor, currentVertex);
                    openSet.Add(downNeighborVertex);
                }
            }

            //left neighbor
            Vector2Int leftNeighbor = new Vector2Int(currentVertex.Point.x - 1, currentVertex.Point.y);

            if (DoesListContainPoint(openSet, leftNeighbor) == false && DoesListContainPoint(closedSet, leftNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, leftNeighbor))
                {
                    SearchVertex leftNeighborVertex = new SearchVertex(leftNeighbor, currentVertex);
                    openSet.Add(leftNeighborVertex);
                }
            }

            //right neighbor
            Vector2Int rightNeighbor = new Vector2Int(currentVertex.Point.x + 1, currentVertex.Point.y);

            if (DoesListContainPoint(openSet, rightNeighbor) == false && DoesListContainPoint(closedSet, rightNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, rightNeighbor))
                {
                    SearchVertex rightNeighborVertex = new SearchVertex(rightNeighbor, currentVertex);
                    openSet.Add(rightNeighborVertex);
                }
            }

            closedSet.Add(currentVertex);
        }

        return new List<Vector2Int>();
    }

    private void ValidateRoom()
    {
        int[,] indexGrid = loadIndexGrid();
        Vector2Int upExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, LevelGenerator.ROOM_HEIGHT - 1);
        Vector2Int downExit = new Vector2Int(LevelGenerator.ROOM_WIDTH / 2, 0);
        Vector2Int leftExit = new Vector2Int(0, LevelGenerator.ROOM_HEIGHT / 2);
        Vector2Int rightExit = new Vector2Int(LevelGenerator.ROOM_WIDTH - 1, LevelGenerator.ROOM_HEIGHT / 2);
        _hasUpExit = IsPointNavigable(indexGrid, upExit);
        _hasDownExit = IsPointNavigable(indexGrid, downExit);
        _hasLeftExit = IsPointNavigable(indexGrid, leftExit);
        _hasRightExit = IsPointNavigable(indexGrid, rightExit);
        _hasUpLeftPath = DoesPathExist(indexGrid, upExit, leftExit);

        if (_hasUpLeftPath)
        {
            Debug.Log("has up left path");
            List<Vector2Int> path = GetPathDFS(indexGrid, upExit, leftExit);
            foreach (Vector2Int point in path)
            {
                Debug.Log("Point: " + point.x + "," + point.y);
            }
        }

        _hasUpRightPath = DoesPathExist(indexGrid, upExit, rightExit);
        _hasUpDownPath = DoesPathExist(indexGrid, upExit, downExit);
        _hasRightDownPath = DoesPathExist(indexGrid, rightExit, downExit);
        _hasRightLeftPath = DoesPathExist(indexGrid, rightExit, leftExit);
        _hasDownLeftPath = DoesPathExist(indexGrid, downExit, leftExit);
        Debug.Log("Room: " + designedRoomFile.name);
        Debug.Log("hasUpexit: " + _hasUpExit);
        Debug.Log("_hasDownExit: " + _hasDownExit);
        Debug.Log("_hasLeftExit: " + _hasLeftExit);
        Debug.Log("_hasRightExit: " + _hasRightExit);
        Debug.Log("_hasUpLeftPath: " + _hasUpLeftPath);
        Debug.Log("_hasUpRightPath: " + _hasUpRightPath);
        Debug.Log("_hasUpDownPath: " + _hasUpDownPath);
        Debug.Log("_hasRightDownPath: " + _hasRightDownPath);
        Debug.Log("_hasRightLeftPath: " + _hasRightLeftPath);
        Debug.Log("_hasDownLeftPath: " + _hasDownLeftPath);
    }

    public bool MeetsConstraints(ExitConstraint requiredExits)
    {
        ValidateRoom();
        if (requiredExits.upExitRequired && _hasUpExit == false)
            return false;
        if (requiredExits.downExitRequired && _hasDownExit == false)
            return false;
        if (requiredExits.leftExitRequired && _hasLeftExit == false)
            return false;
        if (requiredExits.rightExitRequired && _hasRightExit == false)
            return false;
        if (_hasUpLeftPath == false && requiredExits.upExitRequired && requiredExits.leftExitRequired)
            return false;
        if (_hasUpRightPath == false && requiredExits.upExitRequired && requiredExits.rightExitRequired)
            return false;
        if (_hasUpDownPath == false && requiredExits.upExitRequired && requiredExits.downExitRequired)
            return false;
        if (_hasRightDownPath == false && requiredExits.rightExitRequired && requiredExits.downExitRequired)
            return false;
        if (_hasRightLeftPath == false && requiredExits.rightExitRequired && requiredExits.leftExitRequired)
            return false;
        if (_hasDownLeftPath == false && requiredExits.downExitRequired && requiredExits.leftExitRequired)
            return false;
        return true;
    }
    bool IsPointInGrid(Vector2Int point)
    {
        if (point.x < 0 || point.x >= LevelGenerator.ROOM_WIDTH)
            return false;
        if (point.y < 0 || point.y >= LevelGenerator.ROOM_HEIGHT)
            return false;
        return true;
    }
    bool IsPointNavigable(int[,] indexGrid, Vector2Int point)
    {
        if (IsPointInGrid(point) == false)
            return false;
        if (indexGrid[point.x, point.y] == 1)
            return false;
        else
            return true;
    }

    //BFS algorithm, useful for basic pathfinding
    bool DoesPathExist(int[,] indexGrid, Vector2Int startPoint, Vector2Int endPoint)
    {
        List<Vector2Int> openSet = new List<Vector2Int>();
        List<Vector2Int> closedSet = new List<Vector2Int>();
        if (IsPointNavigable(indexGrid, startPoint))
            openSet.Add(startPoint);
        while (openSet.Count > 0)
        {
            Vector2Int currentPoint = openSet[0];
            openSet.RemoveAt(0);
            if (currentPoint == endPoint)
                return true;
            //up neighbor
            Vector2Int upNeighbor = new Vector2Int(currentPoint.x, currentPoint.y + 1);
            if (openSet.Contains(upNeighbor) == false && closedSet.Contains(upNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, upNeighbor))
                    openSet.Add(upNeighbor);
            }
            //down neighbor
            Vector2Int downNeighbor = new Vector2Int(currentPoint.x, currentPoint.y - 1);
            if (openSet.Contains(downNeighbor) == false && closedSet.Contains(downNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, downNeighbor))
                    openSet.Add(downNeighbor);
            }
            //left neighbor
            Vector2Int leftNeighbor = new Vector2Int(currentPoint.x - 1, currentPoint.y);
            if (openSet.Contains(leftNeighbor) == false && closedSet.Contains(leftNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, leftNeighbor))
                    openSet.Add(leftNeighbor);
            }
            //right neighbor
            Vector2Int rightNeighbor = new Vector2Int(currentPoint.x + 1, currentPoint.y);
            if (openSet.Contains(rightNeighbor) == false && closedSet.Contains(rightNeighbor) == false)
            {
                if (IsPointNavigable(indexGrid, rightNeighbor))
                    openSet.Add(rightNeighbor);
            }
            closedSet.Add(currentPoint);
        }
        return false;
    }



    public int[,] loadIndexGrid()
    {
        string initialGridString = designedRoomFile.text;
        string[] rows = initialGridString.Trim().Split('\n');
        int width = rows[0].Trim().Split(',').Length;
        int height = rows.Length;
        if (height != LevelGenerator.ROOM_HEIGHT)
        {
            throw new UnityException(string.Format("Error in room by {0}. Wrong height, Expected: {1}, Got: {2}", roomAuthor, LevelGenerator.ROOM_HEIGHT, height));
        }
        if (width != LevelGenerator.ROOM_WIDTH)
        {
            throw new UnityException(string.Format("Error in room by {0}. Wrong width, Expected: {1}, Got: {2}", roomAuthor, LevelGenerator.ROOM_WIDTH, width));
        }
        int[,] indexGrid = new int[width, height];
        for (int r = 0; r < height; r++)
        {
            string row = rows[height - r - 1];
            string[] cols = row.Trim().Split(',');
            for (int c = 0; c < width; c++)
            {
                indexGrid[c, r] = int.Parse(cols[c]);
            }
        }
        return indexGrid;
    }


}
