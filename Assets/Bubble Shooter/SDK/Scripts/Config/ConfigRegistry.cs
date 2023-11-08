using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigRegistry : MonoBehaviour
{
    [SerializeField] private List<BaseConfig> configsInGame;

    private void Awake()
    {
        InitialiseConfigsInGame();
    }

    private void InitialiseConfigsInGame()
    {
        foreach (BaseConfig config in configsInGame)
        {
            config.Refresh();
        }
    }
}
