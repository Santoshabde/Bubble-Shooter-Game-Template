using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoalTarget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI targetValue;

    public int currentTargetValue;

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
    }
}
