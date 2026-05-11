using UnityEngine;

public class PuzzleReward : MonoBehaviour
{
    public GameObject rewardObject;
    public GameObject rewardMessageUI;

    private bool rewardGiven = false;

    public void GiveReward()
    {
        if (rewardGiven) return;

        rewardGiven = true;

        if (rewardObject != null)
        {
            rewardObject.SetActive(true);
        }

        if (rewardMessageUI != null)
        {
            rewardMessageUI.SetActive(true);
        }

        Debug.Log("Puzzle reward given.");
    }
}