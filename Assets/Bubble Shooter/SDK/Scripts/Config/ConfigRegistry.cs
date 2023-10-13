using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigRegistry : MonoBehaviour
{
    [SerializeField] private List<BaseScriptable> configsInGame;
    
    void Awake()
    {
        foreach (var item in configsInGame)
        {
            item.Init();
        }
    }
}
