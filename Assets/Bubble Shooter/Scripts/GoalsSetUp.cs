using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalsSetUp : MonoBehaviour
{
    [SerializeField] private InGameBubblesData inGameBubbleData;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GoalTarget goalTarget;

    private Dictionary<BubbleType, GoalTarget> currentTargetData;

    public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
    {
        currentTargetData = new Dictionary<BubbleType, GoalTarget>();
        foreach (var item in targetBubbles)
        {
            GoalTarget spawnedTarget = Instantiate(goalTarget);
            spawnedTarget.transform.SetParent(parentTransform);
            spawnedTarget.transform.gameObject.SetActive(true);

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
                currentTargetData[bubble.BubbleColor].UpdateTarget(currentTargetData[bubble.BubbleColor].currentTargetValue - 1);
            }
        }
    }
}
