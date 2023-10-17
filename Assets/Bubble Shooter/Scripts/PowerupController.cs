using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.BubbleShooter;
using UnityEngine.UI;
using System;

public class PowerupController : MonoBehaviour
{
    [SerializeField] private Button powerUp_BombButton;
    [SerializeField] private Button powerUp_multiColoredButton;

    public static Action<BubbleType> OnPowerButtonClicked;

    private void Awake()
    {
        powerUp_BombButton.onClick.AddListener(() => ActivatePowerup(BubbleType.PowerUp_Bomb));
        powerUp_multiColoredButton.onClick.AddListener(() => ActivatePowerup(BubbleType.PowerUp_Colored));
    }

    public void ActivatePowerup(BubbleType bubbleType)
    {
        OnPowerButtonClicked?.Invoke(bubbleType);
    }
}
