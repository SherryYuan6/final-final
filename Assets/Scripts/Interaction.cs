using UnityEngine;
using TMPro;
public class Interaction : MonoSingleton <Interaction>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public static Interaction Instance;

    public GameObject Panel;
    public TextMeshProUGUI Text;
    public TextMeshProUGUI objectText;
    public float fadeSpeed = 2f;
    private CanvasGroup cGroup;
    private bool shouldShow = false;
    // Update is called once per frame
    //void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);
    //}

    private void Start()
    {
        if (Panel == null)
        {
            Debug.LogWarning("Interaction Panel is not assigned.");
            return;
        }

        cGroup = Panel.GetComponent<CanvasGroup>();
        if (cGroup == null)
            cGroup = Panel.AddComponent<CanvasGroup>();
        cGroup.alpha = 0f;
    }

    private void Update()
    {
        if (cGroup == null)
            return;
        float tAlpha;
        if (shouldShow == true)
        {
            tAlpha = 1f;
        }
        else
        {
            tAlpha = 0f;
        }
        if (Mathf.Abs(cGroup.alpha - tAlpha) < 0.01f)
        {
            cGroup.alpha = Mathf.Lerp(cGroup.alpha, tAlpha, Time.deltaTime * fadeSpeed);
        }
        else
        {
            cGroup.alpha = tAlpha; // snap when close
        }
    }

    public void ShowPrompt(string action, string objectName)
    {
        shouldShow = true;
        if (Text != null) 
            Text.text = action;
        if (objectText != null) 
            objectText.text = objectName;
    }

    public void HidePrompt()
    {
        shouldShow = false;
    }
    public void ForceHide()
    {
        shouldShow = false;

        if (cGroup != null)
            cGroup.alpha = 0f;
    }
}
