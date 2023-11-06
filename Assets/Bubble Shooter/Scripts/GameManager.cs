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
        [SerializeField] private VFXData vfxData;

        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private InGameUIManager inGameUIManager;

        public InGameLevelData InGameLevelData => levelGenerationData;
        public ScoreController ScoreController => scoreController;
        public LevelGenerator LevelGenerator => levelGenerator;
        public InGameUIManager InGameUIManager => inGameUIManager;
        public VFXData VFXData => vfxData;

        public LevelGenData currentLevelGenData;
        public bool currentGameStateIsInProgress = false;

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
            scoreController.UpdateGameTargetsScore(bubblesToCalculateScoreFor, () => SwitchState(new GameSuccess(this)));
        }

        public override void SwitchState(State newState)
        {
            base.SwitchState(newState);

            if (Type.Equals(typeof(GameProgress), newState.GetType()))
                currentGameStateIsInProgress = true;
            else
                currentGameStateIsInProgress = false;
        }
    }
}