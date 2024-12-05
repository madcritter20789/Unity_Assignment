using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IAI
{
    public Transform player;
    public float moveSpeed = 3f;
    private AStarPathfinding pathfinder;
    private bool isMoving;
    private List<Vector3> path;

    void Start()
    {
        pathfinder = FindObjectOfType<AStarPathfinding>();
    }

    void Update()
    {
        if (!isMoving)
        {
            MoveToPlayer();
        }
    }

    public void MoveToPlayer()
    {
        Vector3 targetPosition = GetClosestAdjacentTile(player.position);
        if (targetPosition != transform.position)
        {
            path = pathfinder.FindPath(transform.position, targetPosition);
            if (path != null && path.Count > 0)
            {
                StartCoroutine(MoveAlongPath());
            }
        }
    }

    Vector3 GetClosestAdjacentTile(Vector3 playerPosition)
    {
        Vector2Int playerGridPos = pathfinder.WorldToGridPosition(playerPosition);
        Vector2Int[] adjacentOffsets = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var offset in adjacentOffsets)
        {
            Vector2Int adjacentTile = playerGridPos + offset;
            if (pathfinder.IsTileWithinBounds(adjacentTile) && !pathfinder.obstacleData.GetTile(adjacentTile.x, adjacentTile.y).isObstacle)
            {
                return pathfinder.GridToWorldPosition(adjacentTile);
            }
        }
        return transform.position;
    }

    IEnumerator MoveAlongPath()
    {
        isMoving = true;
        foreach (Vector3 position in path)
        {
            while (Vector3.Distance(transform.position, position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = position; // Snap to position
        }
        isMoving = false;
    }
}
