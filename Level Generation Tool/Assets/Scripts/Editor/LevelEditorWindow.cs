using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

// Observer class for managing events
public class LevelEventManager
{
    public static event Action<string> OnWordAdded;
    public static event Action<string> OnWordRemoved;
    public static event Action<string> OnCorrectWordAdded;
    public static event Action<string> OnCorrectWordRemoved;
    public static event Action OnCorrectSelection;
    public static event Action OnIncorrectSelection;

    public static void WordAdded(string word)
    {
        OnWordAdded?.Invoke(word);
    }

    public static void WordRemoved(string word)
    {
        OnWordRemoved?.Invoke(word);
    }

    public static void CorrectWordAdded(string word)
    {
        OnCorrectWordAdded?.Invoke(word);
    }

    public static void CorrectWordRemoved(string word)
    {
        OnCorrectWordRemoved?.Invoke(word);
    }

    public static void CorrectSelection()
    {
        OnCorrectSelection?.Invoke();
    }

    public static void IncorrectSelection()
    {
        OnIncorrectSelection?.Invoke();
    }
}

// Handles validation logic
public class SelectionValidator
{
    private readonly LevelData levelData;

    public SelectionValidator(LevelData data)
    {
        levelData = data;
    }

    public void ValidateSelection(string word)
    {
        if (levelData.correctWords.Contains(word))
        {
            LevelEventManager.CorrectSelection();
        }
        else
        {
            LevelEventManager.IncorrectSelection();
        }
    }
}

// Data Handler for Level Data operations
public class LevelDataHandler
{
    private LevelData levelData;

    public LevelDataHandler(LevelData data)
    {
        levelData = data;
    }

    public void AddWord(string word)
    {
        if (!string.IsNullOrEmpty(word))
        {
            levelData.words.Add(word);
            LevelEventManager.WordAdded(word);
        }
    }

    public void RemoveWord(int index)
    {
        if (index >= 0 && index < levelData.words.Count)
        {
            string word = levelData.words[index];
            levelData.words.RemoveAt(index);
            LevelEventManager.WordRemoved(word);
        }
    }

    public void AddCorrectWord(string word)
    {
        if (!string.IsNullOrEmpty(word))
        {
            levelData.correctWords.Add(word);
            LevelEventManager.CorrectWordAdded(word);
        }
    }

    public void RemoveCorrectWord(int index)
    {
        if (index >= 0 && index < levelData.correctWords.Count)
        {
            string word = levelData.correctWords[index];
            levelData.correctWords.RemoveAt(index);
            LevelEventManager.CorrectWordRemoved(word);
        }
    }
}

// Level Editor Window
public class LevelEditorWindow : EditorWindow
{
    private LevelData levelData;
    private LevelDataHandler levelDataHandler;
    private SelectionValidator selectionValidator;
    private string newWord;
    private string newCorrectWord;
    private string newLevelName;

    [MenuItem("Tools/LevelEditorWindow")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<LevelEditorWindow>();
        wnd.titleContent = new GUIContent("Custom Level Editor");
    }

    private void OnEnable()
    {
        LevelEventManager.OnWordAdded += OnWordAdded;
        LevelEventManager.OnWordRemoved += OnWordRemoved;
        LevelEventManager.OnCorrectWordAdded += OnCorrectWordAdded;
        LevelEventManager.OnCorrectWordRemoved += OnCorrectWordRemoved;
        LevelEventManager.OnCorrectSelection += OnCorrectSelection;
        LevelEventManager.OnIncorrectSelection += OnIncorrectSelection;
    }

    private void OnDisable()
    {
        LevelEventManager.OnWordAdded -= OnWordAdded;
        LevelEventManager.OnWordRemoved -= OnWordRemoved;
        LevelEventManager.OnCorrectWordAdded -= OnCorrectWordAdded;
        LevelEventManager.OnCorrectWordRemoved -= OnCorrectWordRemoved;
        LevelEventManager.OnCorrectSelection -= OnCorrectSelection;
        LevelEventManager.OnIncorrectSelection -= OnIncorrectSelection;
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);
        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData != null)
        {
            if (levelDataHandler == null || levelDataHandler != new LevelDataHandler(levelData))
                levelDataHandler = new LevelDataHandler(levelData);

            if (selectionValidator == null || selectionValidator != new SelectionValidator(levelData))
                selectionValidator = new SelectionValidator(levelData);

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
                levelDataHandler.RemoveWord(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        newWord = EditorGUILayout.TextField("Add New Word", newWord);
        if (GUILayout.Button("Add Word"))
        {
            levelDataHandler.AddWord(newWord);
            newWord = string.Empty;
        }

        EditorGUILayout.LabelField("Correct Words:");
        for (int i = 0; i < levelData.correctWords.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            levelData.correctWords[i] = EditorGUILayout.TextField("Correct Word " + (i + 1), levelData.correctWords[i]);

            if (GUILayout.Button("Delete", GUILayout.Width(60)))
            {
                levelDataHandler.RemoveCorrectWord(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        newCorrectWord = EditorGUILayout.TextField("Add New Correct Word", newCorrectWord);
        if (GUILayout.Button("Add Correct Word"))
        {
            levelDataHandler.AddCorrectWord(newCorrectWord);
            newCorrectWord = string.Empty;
        }

        levelData.animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", levelData.animationClip, typeof(AnimationClip), false);

        if (GUILayout.Button("Save Level Data"))
        {
            SaveLevelData();
        }

        if (GUILayout.Button("Create New Level"))
        {
            CreateLevel();
        }

        // Sample validation for testing
        if (GUILayout.Button("Validate Selection"))
        {
            string wordToValidate = EditorGUILayout.TextField("Word to Validate", "");
            selectionValidator.ValidateSelection(wordToValidate);
        }
    }

    private void SaveLevelData()
    {
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();
    }

    private void CreateLevel()
    {
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        SaveLevel(newScene);
    }

    private void SaveLevel(Scene scene)
    {
        string path = EditorUtility.SaveFilePanelInProject("Save New Level", "NewLevel", "unity", "Specify where to save the new level.");

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
    }

    private void OnWordAdded(string word)
    {
        Debug.Log("Word Added: " + word);
    }

    private void OnWordRemoved(string word)
    {
        Debug.Log("Word Removed: " + word);
    }

    private void OnCorrectWordAdded(string word)
    {
        Debug.Log("Correct Word Added: " + word);
    }

    private void OnCorrectWordRemoved(string word)
    {
        Debug.Log("Correct Word Removed: " + word);
    }

    private void OnCorrectSelection()
    {
        Debug.Log("Correct selection made.");
    }

    private void OnIncorrectSelection()
    {
        Debug.Log("Incorrect selection made.");
    }
}
