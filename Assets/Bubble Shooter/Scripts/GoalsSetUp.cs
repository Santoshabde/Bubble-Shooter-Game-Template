using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalsSetUp : MonoBehaviour
{
    [SerializeField] private InGameBubblesData inGameBubbleData;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GoalTarget goalTarget;

    public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
    {
        foreach (var item in targetBubbles)
        {
            GoalTarget spawnedTarget = Instantiate(goalTarget);
            spawnedTarget.transform.SetParent(parentTransform);
            spawnedTarget.transform.gameObject.SetActive(true);

            spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble]);
        }
    }
}
