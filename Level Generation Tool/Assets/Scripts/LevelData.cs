using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "PuzzleGame/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName; // The name of the level
    public List<string> words = new List<string>(); // List of all possible words
    public List<string> correctWords = new List<string>(); // List of correct words needed to solve the level
    public AnimationClip animationClip; // Primary animation clip for the scene

    public List<AnimationClip> actions = new List<AnimationClip>(); // List of actions/animations triggered upon solving the level
}
