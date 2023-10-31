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
       Destroy(this.gameObject, 3f);
    }

    public void SpawnScoreMesh(int score/*, TextMeshPro toMovePosition*/)
    {
        scoreText.text = "+" + score.ToString();

        Sequence moveSeq = DOTween.Sequence();
        moveSeq.Join(transform.DOScale(transform.localScale + (0.1f * Vector3.one), 0.4f));
        //moveSeq.Join(transform.DOMove(transform.position + new Vector3(0, 0.5f, 0), 0.6f));
        //moveSeq.Append(transform.DOMove(toMovePosition.GetComponent<RectTransform>().position, 0.3f));
        moveSeq.Join(scoreText.DOFade(0, 0.65f));
    }
}
