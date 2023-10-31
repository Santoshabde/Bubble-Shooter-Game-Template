using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SNGames.BubbleShooter;

public class HUDController : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private TextMeshProUGUI gameTime;
    [SerializeField] private TextMeshProUGUI racoonsToRescue;
    [SerializeField] private GoalsSetUp goalsetup;

    public void UpdateTimer(string timer)
    {
        gameTime.text = timer;
    }

    public void UpdateRacoonsToRescue(int racoonsToRescue)
    {
        this.racoonsToRescue.text = racoonsToRescue.ToString();
    }

    public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
    {
        goalsetup.InitialGoalSetUp(targetBubbles);
    }
}
