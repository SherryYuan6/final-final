using UnityEngine;

public class FormulaClueUI : MonoBehaviour
{
    public GameObject formulaCluePanel;
    public GameObject wirePuzzlePanel;

    public void StartWirePuzzle()
    {
        if (formulaCluePanel != null)
            formulaCluePanel.SetActive(false);

        if (wirePuzzlePanel != null)
            wirePuzzlePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseFormulaClue()
    {
        if (formulaCluePanel != null)
            formulaCluePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}