using UnityEngine;

public class NumberPuzzleManager : MonoBehaviour
{
    public DropSlot[] slots;
    public NumberBlock[] blocks;

    public GameObject puzzlePanel;
    public GameObject rewardPanel;

    public NumberPuzzleInteract interactScript;

    private bool solved = false;

    void Update()
    {
        if ((puzzlePanel != null && puzzlePanel.activeSelf) ||
            (rewardPanel != null && rewardPanel.activeSelf))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseUI();
            }
        }
    }

    public void CheckAnswer()
    {
        if (solved)
            return;

        foreach (DropSlot slot in slots)
        {
            if (!slot.HasBlock())
                return;

            if (!slot.IsCorrect())
                return;
        }

        solved = true;
        Debug.Log("Puzzle solved!");

        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);
    }

    public bool IsSolved()
    {
        return solved;
    }

    public void ResetPuzzle()
    {
        if (solved)
            return;

        foreach (DropSlot slot in slots)
        {
            slot.ClearSlot();
        }

        foreach (NumberBlock block in blocks)
        {
            block.ResetToStart();
        }
    }

    public void CloseUI()
    {
        if (interactScript != null)
            interactScript.CloseAllPanels();
    }
}