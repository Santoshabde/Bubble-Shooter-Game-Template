using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.CommonModule;
using TMPro;
using DG.Tweening;

public class ScoreController : SerializeSingleton<ScoreController>
{
    [SerializeField] private BubbleScore scoreIndicator;
    [SerializeField] private Transform scoreContainer;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshPro finalScoreMovePosition;

    [SerializeField, ReadOnly]
    private int totalGameScore = 0;

    private void Start()
    {
        totalGameScore = 0;
    }

    public void UpdateGameScore(int updateScore, Vector3 bubblePosition, bool shouldSpawnText)
    {
        if(!shouldSpawnText)
        {
            BubbleScore score = Instantiate(scoreIndicator, bubblePosition, Quaternion.identity);
            score.SpawnScoreMesh(updateScore, finalScoreMovePosition);
        }

        totalGameScore += updateScore;
        if (scoreText != null)
        {
            scoreText.text = totalGameScore.ToString();

            Sequence containerSeq = DOTween.Sequence();
            containerSeq.AppendInterval(0.9f);
            containerSeq.Append(scoreContainer.transform.DOScale(Vector3.one * 1.14f, 0.13f));
            containerSeq.Append(scoreContainer.transform.DOScale(Vector3.one, 0.13f));
        }
    }
}
