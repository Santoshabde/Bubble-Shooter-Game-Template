using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GoalTarget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI targetValue;

    public int currentTargetValue;

    private Sequence scaleSeq;
    private bool scaleSeqPlaying = false;

    public void SetTarget(int target, Sprite sprite)
    {
        targetValue.text = target.ToString();
        image.sprite = sprite;

        currentTargetValue = target;
    }

    public void UpdateTarget(int value)
    {
        if (value == 0 || value < 0)
            value = 0;

        currentTargetValue = value;
        targetValue.text = value.ToString();

        if (!scaleSeqPlaying)
        {
            scaleSeqPlaying = true;
            scaleSeq = DOTween.Sequence();
            scaleSeq.Append(targetValue.transform.DOScale(1.4f, 0.25f));
            scaleSeq.Append(targetValue.transform.DOScale(1f, 0.25f));
            scaleSeq.OnComplete(() => scaleSeqPlaying = false);
        }
    }
}
