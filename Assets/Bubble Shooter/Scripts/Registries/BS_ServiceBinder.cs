using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class BS_ServiceBinder : ServiceBinder
    {
        protected override void BindAllServicesInGame()
        {
            Debug.Log("Ser Called");

            //Register all in game services here
            ServiceRegistry.Bind(new LevelGenerator());

            ServiceRegistry.Init();
        }
    }
}
