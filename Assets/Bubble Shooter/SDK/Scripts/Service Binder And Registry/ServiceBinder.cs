using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ServiceBinder : MonoBehaviour
{
    protected static Dictionary<Type, BaseService> servicesInGame;

    private void Awake()
    {
        BindAllServicesInGame();
    }

    /// <summary>
    /// Binds All Services at the start of the game
    /// </summary>
    protected virtual void BindAllServicesInGame()
    {
        
    }

    protected void BindService(BaseService service)
    {
        if (servicesInGame == null)
            servicesInGame = new Dictionary<Type, BaseService>();

        Type type = service.GetType();

        if (!servicesInGame.ContainsKey(type))
            servicesInGame.Add(type, service);
        else
            servicesInGame[type] = service;
    }

    public static BaseService GetService<T>()
    {
        Type type = typeof(T);

        if (servicesInGame == null) return null;
        else
        {
            if (servicesInGame.ContainsKey(type))
                return servicesInGame[type];
            else
            {
                Debug.LogError("Service not Binded: " + type);
                return null;
            }
        }
    }
}
