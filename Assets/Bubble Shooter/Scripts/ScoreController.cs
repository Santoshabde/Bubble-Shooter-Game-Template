using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.CommonModule;

public class ScoreController : SerializeSingleton<ScoreController>
{
    [SerializeField] private BubbleScore scoreIndicator;

    [SerializeField, ReadOnly]
    private int totalGameScore = 0;

    private void Start()
    {
        totalGameScore = 0;
    }

    public void UpdateGameScore(int updateScore, Vector3 bubblePosition, bool isFreeBubble)
    {
        if(!isFreeBubble)
        {
            BubbleScore score = Instantiate(scoreIndicator, bubblePosition, Quaternion.identity);
            score.SpawnScoreMesh(updateScore);
        }

        totalGameScore += updateScore;
    }
}
