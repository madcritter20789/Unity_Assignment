using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;
using UnityEditor.SceneManagement;

public class LevelEditorWindow : EditorWindow
{
    private LevelData levelData;
    private string newWord;
    private string newCorrectWord;
    private string newLevelName;

    [MenuItem("Tools/LevelEditorWindow")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<LevelEditorWindow>();
        wnd.titleContent = new GUIContent("Custom Level Editor");
    }

    [MenuItem("PuzzleGame/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);

        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData != null)
        {
            DisplayLevelData();
        }
        else
        {
            EditorGUILayout.HelpBox("Please assign a LevelData asset.", MessageType.Warning);
        }
    }

    private void DisplayLevelData()
    {
        levelData.levelName = EditorGUILayout.TextField("Level Name", levelData.levelName);

        EditorGUILayout.LabelField("Words:");
        for (int i = 0; i < levelData.words.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.words[i] = EditorGUILayout.TextField("Word " + (i + 1), levelData.words[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                RemoveWord(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        newWord = EditorGUILayout.TextField("Add New Word", newWord);
        if (GUILayout.Button("Add Word"))
        {
            AddWord();
        }

        EditorGUILayout.LabelField("Correct Words:");
        for (int i = 0; i < levelData.correctWords.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.correctWords[i] = EditorGUILayout.TextField("Correct Word " + (i + 1), levelData.correctWords[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                RemoveCorrectWord(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        newCorrectWord = EditorGUILayout.TextField("Add New Correct Word", newCorrectWord);
        if (GUILayout.Button("Add Correct Word"))
        {
            AddCorrectWord();
        }

        levelData.animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", levelData.animationClip, typeof(AnimationClip), false);

        EditorGUILayout.LabelField("Actions:");
        for (int i = 0; i < levelData.actions.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.actions[i] = (AnimationClip)EditorGUILayout.ObjectField("Action " + (i + 1), levelData.actions[i], typeof(AnimationClip), false);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                RemoveAction(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Action"))
        {
            AddAction();
        }

        if (GUILayout.Button("Save Level Data"))
        {
            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Create New Level"))
        {
            CreateLevel();
        }

        if (GUILayout.Button("Preview Level"))
        {
            PreviewLevel();
        }
    }

    private void AddWord()
    {
        if (!string.IsNullOrEmpty(newWord))
        {
            levelData.words.Add(newWord);
            newWord = string.Empty;
        }
    }

    private void AddCorrectWord()
    {
        if (!string.IsNullOrEmpty(newCorrectWord))
        {
            levelData.correctWords.Add(newCorrectWord);
            newCorrectWord = string.Empty;
        }
    }

    private void AddAction()
    {
        levelData.actions.Add(null);
    }

    private void RemoveWord(int index)
    {
        if (index >= 0 && index < levelData.words.Count)
        {
            levelData.words.RemoveAt(index);
        }
    }

    private void RemoveCorrectWord(int index)
    {
        if (index >= 0 && index < levelData.correctWords.Count)
        {
            levelData.correctWords.RemoveAt(index);
        }
    }

    private void RemoveAction(int index)
    {
        if (index >= 0 && index < levelData.actions.Count)
        {
            levelData.actions.RemoveAt(index);
        }
    }

    private void CreateLevel()
    {
        //Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        //Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Create a new scene with default game objects (e.g., camera, light)
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        Debug.Log("New level created.");

        // Prompt user to save the scene
        SaveLevel(newScene);
    }


    private void SaveLevel(Scene scene)
    {
        // Prompt the user to select a location and name for the new scene file
        string path = EditorUtility.SaveFilePanelInProject("Save New Level", "NewLevel", "unity", "Assets/Scenes/Levels");

        if (!string.IsNullOrEmpty(path))
        {
            if (EditorSceneManager.SaveScene(scene, path))
            {
                Debug.Log("Level saved successfully at: " + path);
            }
            else
            {
                Debug.LogError("Failed to save the level.");
            }
        }
        else
        {
            Debug.LogWarning("Save level operation canceled.");
        }
    }

    private void PreviewLevel()
    {
        Debug.Log("Previewing Level: " + levelData.levelName);
        EditorSceneManager.NewPreviewScene();
    }
}
