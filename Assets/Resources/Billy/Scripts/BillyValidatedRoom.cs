using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillyValidatedRoom : Room
{
    bool _hasLeftExit;
    bool _hasRightExit;
    bool _hasUpExit;
    bool _hasDownExit;

    bool _hasUpRightPath;
    bool _hasUpDownPath;
    bool _hasUpLeftPath;
    bool _hasDownRightPath;
    bool _hasDownLeftPath;
    bool _hasRightLeftPath;

    List<Vector2Int> _checkedNodes = new List<Vector2Int>();

    public bool MeetsConstraints(ExitConstraint requiredExits)
    {
        int[,] indexGrid = loadIndexGrid();
        
        //Make sure every tile in the room has been checked
        for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++)
        {
            for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++)
            {
                Vector2Int currentPoint = new Vector2Int(x, y);
                if (!IsPointNavigable(indexGrid, currentPoint))
                    continue;
                if (_checkedNodes.Contains(currentPoint))
                    continue;
                CheckExitAndPath(indexGrid, currentPoint);
                ResetExits();
            }
        }

        //Check if path exists
        if (requiredExits.upExitRequired && requiredExits.rightExitRequired && !_hasUpRightPath)
            return false;

        if (requiredExits.upExitRequired && requiredExits.downExitRequired && !_hasUpDownPath)
            return false;

        if (requiredExits.upExitRequired && requiredExits.leftExitRequired && !_hasUpLeftPath)
            return false;

        if (requiredExits.downExitRequired && requiredExits.rightExitRequired && !_hasDownRightPath)
            return false;

        if (requiredExits.downExitRequired && requiredExits.leftExitRequired && !_hasDownLeftPath)
            return false;

        if (requiredExits.rightExitRequired && requiredExits.leftExitRequired && !_hasRightLeftPath)
            return false;

        return true;
    }

    public void CheckExitAndPath (int[,] indexGrid, Vector2Int startPoint)
    {
        List<Vector2Int> openSet = new List<Vector2Int>();
        List<Vector2Int> closedSet = new List<Vector2Int>();

        openSet.Add(startPoint);

        //Put all the possible nodes in the closed set
        while (openSet.Count > 0)
        {
            Vector2Int currentPoint = openSet[0];
            openSet.RemoveAt(0);
            closedSet.Add(currentPoint);

            Vector2Int neighbor;
            //Up neighbor
            if (currentPoint.y < LevelGenerator.ROOM_HEIGHT - 1)
            {
                neighbor = new Vector2Int(currentPoint.x, currentPoint.y + 1);
                if (IsPointNavigable(indexGrid, neighbor) &&
                    !openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
            //Down neighbor
            if (currentPoint.y > 0)
            {
                neighbor = new Vector2Int(currentPoint.x, currentPoint.y - 1);
                if (IsPointNavigable(indexGrid, neighbor) &&
                    !openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
            //Right neibor
            if (currentPoint.x < LevelGenerator.ROOM_WIDTH - 1)
            {
                neighbor = new Vector2Int(currentPoint.x + 1, currentPoint.y);
                if (IsPointNavigable(indexGrid, neighbor) &&
                    !openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
            //Left neighbor
            if (currentPoint.x > 0)
            {
                neighbor = new Vector2Int(currentPoint.x - 1, currentPoint.y);
                if (IsPointNavigable(indexGrid, neighbor) &&
                    !openSet.Contains(neighbor) && !closedSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }

        //Check Exit
        foreach (Vector2Int node in closedSet)
        {
            if (node.y == LevelGenerator.ROOM_HEIGHT - 1)
            {
                _hasUpExit = true;
            }
            if (node.y == 0)
            {
                _hasDownExit = true;
            }
            if (node.x == LevelGenerator.ROOM_WIDTH - 1)
            {
                _hasRightExit = true;
            }
            if (node.x == 0)
            {
                _hasLeftExit = true;
            }
        }

        //Check Path
        if (_hasUpExit)
        {
            if (_hasRightExit)
                _hasUpRightPath = true;
            if (_hasDownExit)
                _hasUpDownPath = true;
            if (_hasLeftExit)
                _hasUpLeftPath = true;
        }
        if (_hasDownExit)
        {
            if (_hasRightExit)
                _hasDownRightPath = true;
            if (_hasLeftExit)
                _hasDownLeftPath = true;
        }
        if (_hasRightExit &&_hasLeftExit)
            _hasRightLeftPath = true;

        foreach (Vector2Int node in closedSet)
        {
            _checkedNodes.Add(node);
        }
    }

    void ResetExits ()
    {
        _hasLeftExit = false;
        _hasRightExit = false;
        _hasUpExit = false;
        _hasDownExit = false;
    }

    bool IsPointNavigable (int[,] indexGrid, Vector2Int point)
    {
        if (indexGrid[point.x, point.y] == 1)
            return false;
        return true;
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
