using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Data Editor", EditorStyles.boldLabel);

        // Select ScriptableObject
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);

        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign an Obstacle Data ScriptableObject.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);

        // Draw Grid using 1D Array Indexing
        for (int x = 0; x < 10; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = 0; z < 10; z++)
            {
                int index = x * 10 + z;
                obstacleData.GetTile(x, z).isObstacle = GUILayout.Toggle(obstacleData.GetTile(x, z).isObstacle, "", GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Clear All Button
        if (GUILayout.Button("Clear All"))
        {
            ClearAll();
        }

        // Undo Last Action Button
        if (GUILayout.Button("Undo Last Check"))
        {
            UndoLastCheck();
        }

        // Save Changes Button
        if (GUILayout.Button("Save to Obstacle Data"))
        {
            SaveToObstacleData();
        }
    }

    private void ClearAll()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                obstacleData.SetTile(x, z, false);
            }
        }
    }

    private Vector2 lastCheckedBox = new Vector2(-1, -1); // Track last checked box

    private void UndoLastCheck()
    {
        if (lastCheckedBox.x >= 0 && lastCheckedBox.y >= 0)
        {
            int index = (int)lastCheckedBox.x * 10 + (int)lastCheckedBox.y;
            obstacleData.GetTile((int)lastCheckedBox.x, (int)lastCheckedBox.y).isObstacle = false;
            lastCheckedBox = new Vector2(-1, -1);
        }
    }

    private void SaveToObstacleData()
    {
        EditorUtility.SetDirty(obstacleData);
        AssetDatabase.SaveAssets();
        Debug.Log("Obstacle data saved!");
    }
}


/*
public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;
    private bool[,] tempObstacleGrid = new bool[10, 10];
    private Vector2 lastCheckedBox = new Vector2(-1, -1); // Store the last checked box

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Data Editor", EditorStyles.boldLabel);

        // Select ScriptableObject
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);

        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign an Obstacle Data ScriptableObject.", MessageType.Warning);
            return;
        }

        GUILayout.Space(10);

        // Draw Grid
        for (int x = 0; x < 10; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = 0; z < 10; z++)
            {
                bool previousState = tempObstacleGrid[x, z];
                tempObstacleGrid[x, z] = GUILayout.Toggle(tempObstacleGrid[x, z], "", GUILayout.Width(20));

                // Track the last checked box for Undo
                if (tempObstacleGrid[x, z] && !previousState)
                {
                    lastCheckedBox = new Vector2(x, z);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Clear All Button
        if (GUILayout.Button("Clear All"))
        {
            ClearAll();
        }

        // Undo Last Action Button
        if (GUILayout.Button("Undo Last Check"))
        {
            UndoLastCheck();
        }

        // Save Changes Button
        if (GUILayout.Button("Save to Obstacle Data"))
        {
            SaveToObstacleData();
        }
    }

    private void ClearAll()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                tempObstacleGrid[x, z] = false;
            }
        }
    }

    private void UndoLastCheck()
    {
        if (lastCheckedBox.x >= 0 && lastCheckedBox.y >= 0)
        {
            tempObstacleGrid[(int)lastCheckedBox.x, (int)lastCheckedBox.y] = false;
            lastCheckedBox = new Vector2(-1, -1);
        }
    }

    private void SaveToObstacleData()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                obstacleData.obstacleGrid[x, z] = tempObstacleGrid[x, z];
            }
        }

        EditorUtility.SetDirty(obstacleData);
        AssetDatabase.SaveAssets();
        Debug.Log("Obstacle data saved!");
    }

    private void OnEnable()
    {
        // Load the existing grid into temporary grid when the window opens
        if (obstacleData != null)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    tempObstacleGrid[x, z] = obstacleData.obstacleGrid[x, z];
                }
            }
        }
    }
}


using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;
    private bool[,] tempObstacleGrid = new bool[10, 10];

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnGUI()
    {
        obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);
        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please assign an Obstacle Data ScriptableObject.", MessageType.Warning);
            return;
        }

        for (int x = 0; x < 10; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int z = 0; z < 10; z++)
            {
                tempObstacleGrid[x, z] = GUILayout.Toggle(obstacleData.obstacleGrid[x, z], "", GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save to Obstacle Data"))
        {
            SaveObstacleData();
        }
    }

    private void SaveObstacleData()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                obstacleData.obstacleGrid[x, z] = tempObstacleGrid[x, z];
            }
        }

        EditorUtility.SetDirty(obstacleData);
        AssetDatabase.SaveAssets();
        Debug.Log("Obstacle data saved!");
    }
}
*/