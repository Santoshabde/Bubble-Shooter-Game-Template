using SNGames.BubbleShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalsSetUp : MonoBehaviour
{
    [SerializeField] private InGameBubblesData inGameBubbleData;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GoalTarget goalTarget;
    [SerializeField] private BubbleScore scoreIndicator;

    private Dictionary<BubbleType, GoalTarget> currentTargetData;

    public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
    {
        currentTargetData = new Dictionary<BubbleType, GoalTarget>();
        LevelData.currentLevelCurrentTargetStatus = new Dictionary<BubbleType, int>();

        foreach (var item in targetBubbles)
        {
            GoalTarget spawnedTarget = Instantiate(goalTarget);
            spawnedTarget.transform.SetParent(parentTransform);
            spawnedTarget.transform.gameObject.SetActive(true);
            spawnedTarget.transform.localScale = Vector3.one;

            spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble]);

            currentTargetData.Add(item.targetBubble, spawnedTarget);
            LevelData.currentLevelCurrentTargetStatus.Add(item.targetBubble, item.targetNumber);
        }
    }

    public void UpdateTargetData(List<Bubble> bubblesToCalculateScoreFor, Action OnAllTargetGoalsReached = null)
    {
        foreach (var bubble in bubblesToCalculateScoreFor)
        {
            //Update Dictionary
            if(currentTargetData.ContainsKey(bubble.BubbleColor))
            {
                int currentTargetValue = currentTargetData[bubble.BubbleColor].currentTargetValue;
                int valueAfterUpdating = currentTargetValue - 1;

                if (valueAfterUpdating >= 0)
                    SpawnScoreText(bubble.PositionID);

                if (valueAfterUpdating <= 0)
                {
                    valueAfterUpdating = 0;
                }

                currentTargetData[bubble.BubbleColor].UpdateTarget(valueAfterUpdating);
                LevelData.currentLevelCurrentTargetStatus[bubble.BubbleColor] = valueAfterUpdating;
            }
        }

        //All Targets Reached?
        int count = 0;
        foreach (var item in currentTargetData)
        {
            if(item.Value.currentTargetValue == 0)
            {
                count += 1;
            }
        }

        if (count == currentTargetData.Count)
            OnAllTargetGoalsReached?.Invoke();
    }

    //Only visual purpose
    public void SpawnScoreText(Vector3 positionToSpawn)
    {
        if (scoreIndicator != null)
        {
            BubbleScore score = Instantiate(scoreIndicator, positionToSpawn, Quaternion.identity);
            score.SpawnScoreMesh(1);
        }
    }
}
