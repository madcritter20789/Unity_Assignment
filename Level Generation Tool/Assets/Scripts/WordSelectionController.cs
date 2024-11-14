using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WordSelectionController : MonoBehaviour
{
    public LevelData currentLevelData;
    public UnityEvent onCorrectSelection;

    private List<string> selectedWords = new List<string>();

    public void SelectWord(string word)
    {
        if (selectedWords.Contains(word))
        {
            Debug.Log("Word already selected.");
            return;
        }

        selectedWords.Add(word);

        if (selectedWords.Count >= currentLevelData.correctWords.Count)
        {
            ValidateSelection();
        }
    }

    private void ValidateSelection()
    {
        bool isCorrect = true;
        foreach (var correctWord in currentLevelData.correctWords)
        {
            if (!selectedWords.Contains(correctWord))
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("Correct words selected!");
            onCorrectSelection.Invoke();
        }
        else
        {
            Debug.Log("Incorrect selection. Try again.");
        }

        ResetSelection();
    }

    private void ResetSelection()
    {
        selectedWords.Clear();
    }
}
