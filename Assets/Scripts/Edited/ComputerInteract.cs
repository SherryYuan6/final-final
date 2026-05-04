using UnityEngine;
using TMPro;

public class ComputerInteract : MonoBehaviour
{
    public GameObject passwordPanel;
    public TMP_InputField passwordInputField;

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !passwordPanel.activeSelf)
        {
            OpenPanel();
        }

        if (passwordPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void OpenPanel() {
        if (passwordPanel != null)
        {
            passwordPanel.SetActive(true);
        }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        if (passwordInputField != null) {
            passwordInputField.text = "";
            passwordInputField.Select();
            passwordInputField.ActivateInputField();
        }
    }

    public void ClosePanel()
    {
        if (passwordPanel != null)
        { 
        passwordPanel.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (passwordPanel != null && passwordPanel.activeSelf) 
            {
                ClosePanel();
            }
        }
    }
}