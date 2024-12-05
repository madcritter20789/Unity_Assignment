using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleData obstacleData;
    public GameObject obstaclePrefab;

    void Start()
    {
        if (obstacleData == null)
        {
            Debug.LogError("ObstacleData is not assigned!");
            return;
        }

        GenerateObstacles();
    }

    void GenerateObstacles()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if (obstacleData.GetTile(x, z).isObstacle)
                {
                    Vector3 position = new Vector3(x * 1.1f, 0.5f, z * 1.1f);
                    Instantiate(obstaclePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
