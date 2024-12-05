using UnityEngine;

[System.Serializable]
public class ObstacleTile
{
    public bool isObstacle;
}

[CreateAssetMenu(fileName = "ObstacleData", menuName = "ScriptableObjects/ObstacleData")]
public class ObstacleData : ScriptableObject
{
    [SerializeField] private ObstacleTile[] obstacleGrid = new ObstacleTile[100]; // 10x10 as 1D array

    public ObstacleTile GetTile(int x, int z) => obstacleGrid[x * 10 + z]; // Convert 2D to 1D indexing

    public void SetTile(int x, int z, bool value)
    {
        obstacleGrid[x * 10 + z].isObstacle = value;
    }
}
