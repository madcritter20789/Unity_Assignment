using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int Position;
    public int GCost;
    public int HCost;
    public int FCost => GCost + HCost;
    public Node Parent;

    public Node(Vector2Int position)
    {
        Position = position;
        GCost = int.MaxValue;
        HCost = 0;
    }
}

public class AStarPathfinding : MonoBehaviour
{
    public ObstacleData obstacleData;
    private int gridSize = 10;
    private float tileSpacing = 1.1f;

    public List<Vector3> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
    {
        Vector2Int start = WorldToGridPosition(startWorldPos);
        Vector2Int target = WorldToGridPosition(targetWorldPos);

        if (obstacleData.GetTile(target.x, target.y).isObstacle)
        {
            Debug.Log($"Target position is blocked at: {target}");
            return null;
        }

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Node[,] nodes = new Node[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                nodes[x, y] = new Node(new Vector2Int(x, y));
            }
        }

        Node startNode = nodes[start.x, start.y];
        Node targetNode = nodes[target.x, target.y];
        startNode.GCost = 0;

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            foreach (Node node in openList)
            {
                if (node.FCost < currentNode.FCost || (node.FCost == currentNode.FCost && node.HCost < currentNode.HCost))
                {
                    currentNode = node;
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode, nodes))
            {
                if (closedList.Contains(neighbor) || obstacleData.GetTile(neighbor.Position.x, neighbor.Position.y).isObstacle)
                    continue;

                int newCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.GCost || !openList.Contains(neighbor))
                {
                    neighbor.GCost = newCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dx = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
        int dy = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);
        return (dx > dy) ? 14 * dy + 10 * (dx - dy) : 14 * dx + 10 * (dy - dx);
    }

    private IEnumerable<Node> GetNeighbors(Node currentNode, Node[,] nodes)
    {
        List<Node> neighbors = new List<Node>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = currentNode.Position + direction;
            if (IsTileWithinBounds(neighborPos))
            {
                neighbors.Add(nodes[neighborPos.x, neighborPos.y]);
            }
        }

        return neighbors;
    }

    private List<Vector3> RetracePath(Node startNode, Node targetNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(GridToWorldPosition(currentNode.Position));
            currentNode = currentNode.Parent;
        }
        path.Reverse();
        return path;
    }

    public bool IsTileWithinBounds(Vector2Int tile)
    {
        return tile.x >= 0 && tile.x < gridSize && tile.y >= 0 && tile.y < gridSize;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPos)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPos.x / tileSpacing), Mathf.RoundToInt(worldPos.z / tileSpacing));
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * tileSpacing, 0.5f, gridPos.y * tileSpacing);
    }
}
