using SNGames.BubbleShooter;
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
        foreach (var item in targetBubbles)
        {
            GoalTarget spawnedTarget = Instantiate(goalTarget);
            spawnedTarget.transform.SetParent(parentTransform);
            spawnedTarget.transform.gameObject.SetActive(true);
            spawnedTarget.transform.localScale = Vector3.one;

            spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble]);

            currentTargetData.Add(item.targetBubble, spawnedTarget);
        }
    }

    public void UpdateTargetData(List<Bubble> bubblesToCalculateScoreFor)
    {
        foreach (var bubble in bubblesToCalculateScoreFor)
        {
            //Update Dictionary
            if(currentTargetData.ContainsKey(bubble.BubbleColor))
            {
                if (currentTargetData[bubble.BubbleColor].currentTargetValue - 1 >= 0)
                    SpawnScoreText(bubble.PositionID);

                currentTargetData[bubble.BubbleColor].UpdateTarget(currentTargetData[bubble.BubbleColor].currentTargetValue - 1);
            }
        }
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
