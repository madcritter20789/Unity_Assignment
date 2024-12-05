using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathfinding : MonoBehaviour
{
    public ObstacleData obstacleData;
    private Vector2Int gridSize = new Vector2Int(10, 10);
    private bool[,] visited;

    public List<Vector3> FindPath(Vector3 start, Vector3 target)
    {
        Vector2Int startTile = WorldToGridPosition(start);
        Vector2Int targetTile = WorldToGridPosition(target);

        if (obstacleData.GetTile(targetTile.x, targetTile.y).isObstacle)
        {
            Debug.Log("Target position is blocked.");
            return null;
        }

        visited = new bool[gridSize.x, gridSize.y];
        List<Vector3> path = BreadthFirstSearch(startTile, targetTile);

        return path;
    }

    List<Vector3> BreadthFirstSearch(Vector2Int start, Vector2Int target)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == target)
                return ConstructPath(cameFrom, start, target);

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = current + direction;

                if (IsTileWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y] && !obstacleData.GetTile(neighbor.x, neighbor.y).isObstacle)
                {
                    queue.Enqueue(neighbor);
                    visited[neighbor.x, neighbor.y] = true;
                    cameFrom[neighbor] = current;
                }
            }
        }

        return null;  // No path found
    }

    List<Vector3> ConstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int start, Vector2Int end)
    {
        List<Vector3> path = new List<Vector3>();
        Vector2Int current = end;

        while (current != start)
        {
            path.Add(new Vector3(current.x * 1.1f, 0.5f, current.y * 1.1f));
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }

    bool IsTileWithinBounds(Vector2Int tile)
    {
        return tile.x >= 0 && tile.x < gridSize.x && tile.y >= 0 && tile.y < gridSize.y;
    }

    Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x / 1.1f), Mathf.RoundToInt(worldPosition.z / 1.1f));
    }
}
