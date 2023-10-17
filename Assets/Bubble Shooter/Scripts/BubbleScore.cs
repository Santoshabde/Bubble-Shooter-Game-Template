using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BubbleScore : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;

    private void Start()
    {
        Destroy(this.gameObject, 0.7f);
    }

    public void SpawnScoreMesh(int score)
    {
        scoreText.text = score.ToString();
        transform.DOScale(transform.localScale + (0.1f * Vector3.one), 0.4f);
        transform.DOMove(transform.position + new Vector3(0, 0.5f, 0), 0.6f);
    }
}
