using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PatternLock : MonoBehaviour
{
    [Header("Correct Sequence")]
    public string[] correctSequence = { "Star", "Circle", "Triangle", "Star", "Square", "Circle" };

    [Header("UI")]
    public TMP_Text feedbackText;
    public GameObject lockPanel;
    public GameObject rewardPanel; // 打开后显示盒子里的线索/权限卡

    private List<string> playerInput = new List<string>();

    public void PressSymbol(string symbolName)
    {
        playerInput.Add(symbolName);

        Debug.Log("Pressed: " + symbolName);

        if (feedbackText != null)
        {
            feedbackText.text = "Input: " + playerInput.Count + " / " + correctSequence.Length;
        }

        CheckProgress();
    }

    private void CheckProgress()
    {
        int currentIndex = playerInput.Count - 1;

        // 当前这一步按错了
        if (playerInput[currentIndex] != correctSequence[currentIndex])
        {
            Debug.Log("Wrong pattern");

            if (feedbackText != null)
                feedbackText.text = "Wrong sequence. Try again.";

            ResetInput();
            return;
        }

        // 全部按对
        if (playerInput.Count == correctSequence.Length)
        {
            UnlockBox();
        }
    }

    public void ResetInput()
    {
        playerInput.Clear();

        if (feedbackText != null)
            feedbackText.text = "Input reset.";
    }

    private void UnlockBox()
    {
        Debug.Log("Pattern lock opened!");

        if (feedbackText != null)
            feedbackText.text = "Unlocked.";

        if (lockPanel != null)
            lockPanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);
    }
}