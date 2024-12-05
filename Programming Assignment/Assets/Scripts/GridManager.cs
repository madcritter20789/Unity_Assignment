using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public GameObject playerPrefab; // Reference to the player prefab
    public Text positionText;
    private int gridSize = 10;
    private float spacing = 1.1f;

    private GameObject player;

    void Start()
    {
        GenerateGrid();
        SpawnPlayer();
        SpawnEnemy();
    }

    void Update()
    {
        DetectTileUnderMouse();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing); // Spaced positioning
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, transform); // Set parent to this GameObject
                tile.name = $"Tile_{x}_{z}";
                TileInfo tileInfo = tile.AddComponent<TileInfo>();
                tileInfo.SetTilePosition(x, z);
            }
        }
    }

    void DetectTileUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TileInfo tileInfo = hit.collider.GetComponent<TileInfo>();
            if (tileInfo != null)
            {
                positionText.text = $"Tile Position: {tileInfo.X}, {tileInfo.Z}";
            }
        }
    }

    void SpawnPlayer()
    {
        Vector3 playerPosition = new Vector3((gridSize / 2) * spacing, 0.5f, (gridSize / 2) * spacing);
        player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
    }

    void SpawnEnemy()
    {
        Vector3 enemyPosition = GetRandomValidPosition();
        GameObject enemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

        // Assign player reference to the enemy AI
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.player = player.transform;
        }
    }

    Vector3 GetRandomValidPosition()
    {
        while (true)
        {
            int x = Random.Range(0, gridSize);
            int z = Random.Range(0, gridSize);
            Vector3 position = new Vector3(x * spacing, 0.5f, z * spacing);

            // Ensure the position is not blocked and not the same as the player
            if (position != player.transform.position)
            {
                return position;
            }
        }
    }
}
