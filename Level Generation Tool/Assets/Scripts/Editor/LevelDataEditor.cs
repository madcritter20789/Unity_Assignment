#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelData levelData = (LevelData)target;
        if (GUILayout.Button("Preview Level"))
        {
            // Mock preview actions
            Debug.Log("Previewing Level: " + levelData.levelName);
            foreach (var word in levelData.words)
            {
                Debug.Log("Word: " + word);
            }
        }
    }
}
#endif
