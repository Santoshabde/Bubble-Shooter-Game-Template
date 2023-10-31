using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoalTarget : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI targetValue;

    public void SetTarget(int target, Sprite sprite)
    {
        targetValue.text = target.ToString();
        image.sprite = sprite;
    }
}
