using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class GoalTarget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image onCompleteIcon;
    [SerializeField] private Image onIncompleteIcon;
    [SerializeField] private TextMeshProUGUI targetValue;

    public int currentTargetValue;

    private Sequence scaleSeq;
    private bool scaleSeqPlaying = false;

    public void SetTarget(int target, Sprite sprite, bool shouldEnableCompleteByDefault = false, bool shouldEnableInCompleteByDefault = false)
    {
        if (targetValue != null)
            targetValue.text = target.ToString();

        image.sprite = sprite;

        currentTargetValue = target;

        if (shouldEnableCompleteByDefault && onCompleteIcon != null)
        {
            onCompleteIcon?.gameObject.SetActive(true);
            onIncompleteIcon?.gameObject.SetActive(false);
        }

        if (shouldEnableInCompleteByDefault && onIncompleteIcon != null)
        {
            onIncompleteIcon?.gameObject.SetActive(true);
            onCompleteIcon?.gameObject.SetActive(false);
        }
    }

    public void UpdateTarget(int value)
    {
        if (value == 0 || value < 0)
        {
            onCompleteIcon?.gameObject.SetActive(true);
            targetValue.gameObject.SetActive(false);
            value = 0;
        }

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
