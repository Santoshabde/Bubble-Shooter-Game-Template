using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using System;

namespace SNGames.BubbleShooter
{
    public class GameManager : StateMachine
    {
        [SerializeField] private InGameLevelData levelGenerationData;
        [SerializeField] private HUDController hudController;
        [SerializeField] private LevelGenerator levelGenerator;

        public InGameLevelData InGameLevelData => levelGenerationData;
        public HUDController HudController => hudController;
        public LevelGenerator LevelGenerator => levelGenerator;

        public int currentLevel;
        public LevelGenData currentLevelGenData;

        private void Awake()
        {
            SwitchState(new GameStart(this));
        }
    }
}