using UnityEngine;
using TMPro;

public class ColorSequenceLock : MonoBehaviour
{
    [Header("Correct Sequence")]
    public string[] correctSequence = { "Red", "Blue", "Blue", "Yellow" };

    [Header("Buttons")]
    public ColorButton[] colorButtons;

    [Header("UI")]
    public TMP_Text feedbackText;

    [Header("Unlock Result")]
    public GameObject puzzlePanel;
    public GameObject rewardPanel;
    public GameObject objectToUnlock;

    private int currentIndex = 0;
    private bool solved = false;

    public void PressColor(string colorID)
    {
        if (solved) return;

        Debug.Log("Pressed: " + colorID);

        if (colorID == correctSequence[currentIndex])
        {
            currentIndex++;

            if (feedbackText != null)
                feedbackText.text = "Correct";

            if (currentIndex >= correctSequence.Length)
            {
                Unlock();
            }
        }
        else
        {
            ResetPuzzle();

            if (feedbackText != null)
                feedbackText.text = "Wrong sequence. Try again.";
        }
    }

    void Unlock()
    {
        solved = true;

        if (feedbackText != null)
            feedbackText.text = "Unlocked!";

        if (objectToUnlock != null)
            objectToUnlock.SetActive(true);

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);

        Debug.Log("Color puzzle solved!");
    }

    public void ResetPuzzle()
    {
        currentIndex = 0;

    }
}