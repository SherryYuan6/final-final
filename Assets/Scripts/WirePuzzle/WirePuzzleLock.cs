using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WirePuzzleLock : MonoBehaviour
{
    [Header("Correct Wire Sequence")]
    public string[] correctSequence =
    {
        "Blue",
        "Yellow",
        "Green",
        "Pink",
        "Red"
    };

    [Header("UI References")]
    public TMP_Text feedbackText;
    public GameObject wirePuzzlePanel;
    public GameObject shutdownProtocolPanel;

    private List<string> playerInput = new List<string>();
    private bool isUnlocked = false;

    public void PressWire(string wireColor)
    {
        if (isUnlocked) return;

        playerInput.Add(wireColor);

        Debug.Log("Pressed wire: " + wireColor);

        if (feedbackText != null)
        {
            feedbackText.text = "Input: " + playerInput.Count + " / " + correctSequence.Length;
        }

        CheckCurrentInput();
    }

    private void CheckCurrentInput()
    {
        int currentIndex = playerInput.Count - 1;

        if (playerInput[currentIndex] != correctSequence[currentIndex])
        {
            Debug.Log("Wrong wire sequence.");

            if (feedbackText != null)
            {
                feedbackText.text = "Signal mismatch. Sequence reset.";
            }

            ResetInput();
            return;
        }

        if (playerInput.Count == correctSequence.Length)
        {
            UnlockProtocol();
        }
    }

    public void ResetInput()
    {
        playerInput.Clear();

        if (feedbackText != null && !isUnlocked)
        {
            feedbackText.text = "Input reset.";
        }
    }

    private void UnlockProtocol()
    {
        isUnlocked = true;

        Debug.Log("Wire puzzle solved. Shutdown protocol recovered.");

        if (feedbackText != null)
        {
            feedbackText.text = "Signal restored.";
        }

        if (wirePuzzlePanel != null)
        {
            wirePuzzlePanel.SetActive(false);
        }

        if (shutdownProtocolPanel != null)
        {
            shutdownProtocolPanel.SetActive(true);
        }
    }

    public void CloseWirePuzzle()
    {
        if (wirePuzzlePanel != null)
        {
            wirePuzzlePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void CloseShutdownProtocol()
    {
        if (shutdownProtocolPanel != null)
        {
            shutdownProtocolPanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
