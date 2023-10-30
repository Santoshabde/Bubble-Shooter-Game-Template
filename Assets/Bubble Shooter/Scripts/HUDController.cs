using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Level Data")]
    [SerializeField] private TextMeshProUGUI gameTime;
    [SerializeField] private TextMeshProUGUI racoonsToRescue;

    public void UpdateTimer(string timer)
    {
        gameTime.text = timer;
    }

    public void UpdateRacoonsToRescue(string racoonsToRescue)
    {
        this.racoonsToRescue.text = racoonsToRescue;
    }
}
