using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.CommonModule;
using TMPro;
using DG.Tweening;
using SNGames.BubbleShooter;

public class ScoreController : MonoBehaviour
{
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

    public void UpdateGameTargetsScore(List<Bubble> bubblesToCalculateScoreFor)
    {
        goalsetup.UpdateTargetData(bubblesToCalculateScoreFor);
    }
}
