using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.CommonModule;
using TMPro;
using DG.Tweening;
using SNGames.BubbleShooter;
using System;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTime;
    [SerializeField] private TextMeshProUGUI racoonsToRescue;
    [SerializeField] private GoalsSetUp goalsetup;

    public void UpdateTimer(string timer, bool shouldAnimate = false)
    {
        gameTime.text = timer;

        if (shouldAnimate)
        {
            gameTime.color = Color.red;

            Sequence scaleSeq = DOTween.Sequence();
            scaleSeq.Append(gameTime.transform.DOScale(1.2f, 0.30f));
            scaleSeq.Append(gameTime.transform.DOScale(1f, 0.30f));
        }
    }

    public void UpdateRacoonsToRescue(int racoonsToRescue)
    {
        this.racoonsToRescue.text = racoonsToRescue.ToString();
    }

    public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
    {
        goalsetup.InitialGoalSetUp(targetBubbles);
    }

    public void UpdateGameTargetsScore(List<Bubble> bubblesToCalculateScoreFor, Action OnAllTargetsReached = null)
    {
        goalsetup.UpdateTargetData(bubblesToCalculateScoreFor, OnAllTargetsReached);
    }
}
