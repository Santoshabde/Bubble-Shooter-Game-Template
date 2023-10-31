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

        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private ScoreController scoreController;

        public InGameLevelData InGameLevelData => levelGenerationData;
        public ScoreController ScoreController => scoreController;
        public LevelGenerator LevelGenerator => levelGenerator;

        public int currentLevel;
        public LevelGenData currentLevelGenData;

        public static GameManager Instance;
        private void Awake()
        {
            //Singleton
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

            SwitchState(new GameStart(this));
        }

        public void UpdateGameTargetsScore(List<Bubble> bubblesToCalculateScoreFor)
        {
            scoreController.UpdateGameTargetsScore(bubblesToCalculateScoreFor);
        }
    }
}