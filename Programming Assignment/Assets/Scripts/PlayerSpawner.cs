using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;  // Player prefab (a capsule)
    public ObstacleData obstacleData;  // Reference to ObstacleData
    private int gridSize = 10;  // Grid size
    private Vector3 spawnPosition;

    void Start()
    {
        Vector2Int middleTile = new Vector2Int(gridSize / 2, gridSize / 2);
        spawnPosition = GetValidSpawnPosition(middleTile);

        // Spawn the player at a valid position
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetValidSpawnPosition(Vector2Int tilePosition)
    {
        // If the middle tile is obstructed, find an adjacent tile
        if (obstacleData.GetTile(tilePosition.x, tilePosition.y).isObstacle)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (Vector2Int dir in directions)
            {
                Vector2Int adjacentTile = tilePosition + dir;
                if (IsTileWithinBounds(adjacentTile) && !obstacleData.GetTile(adjacentTile.x, adjacentTile.y).isObstacle)
                {
                    return new Vector3(adjacentTile.x * 1.1f, 0.5f, adjacentTile.y * 1.1f);
                }
            }
        }

        // Default: Return the middle position
        return new Vector3(tilePosition.x * 1.1f, 0.5f, tilePosition.y * 1.1f);
    }

    bool IsTileWithinBounds(Vector2Int tile)
    {
        return tile.x >= 0 && tile.x < gridSize && tile.y >= 0 && tile.y < gridSize;
    }
}

