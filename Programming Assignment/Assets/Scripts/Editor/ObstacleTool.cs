using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleTool : Editor
{
    public override void OnInspectorGUI()
    {
        ObstacleData data = (ObstacleData)target;

        EditorGUILayout.LabelField("Obstacle Grid", EditorStyles.boldLabel);

        for (int x = 0; x < 10; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = 0; z < 10; z++)
            {
                ObstacleTile tile = data.GetTile(x, z);
                tile.isObstacle = EditorGUILayout.Toggle(tile.isObstacle, GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}
