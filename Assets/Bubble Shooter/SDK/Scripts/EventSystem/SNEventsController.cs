using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public static class SNEventsController<T> where T : Enum
    {
        public static Dictionary<T, Action> eventsInGame = new Dictionary<T, Action>();

        public static void RegisterEvent(T eventName, Action actionCallBack)
        {
            if (eventsInGame == null)
                eventsInGame = new Dictionary<T, Action>();

            if (!eventsInGame.ContainsKey(eventName))
                eventsInGame.Add(eventName, actionCallBack);
            else
                eventsInGame[eventName] += actionCallBack;
        }

        public static void TriggerEvent(T eventName)
        {
            if (!eventsInGame.ContainsKey(eventName))
                return;

            eventsInGame[eventName]?.Invoke();
        }

        public static void DeregisterEvent(T eventName, Action listerner)
        {
            eventsInGame[eventName] -= listerner;
        }
    }
}
