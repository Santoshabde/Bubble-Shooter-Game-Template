using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.BubbleShooter
{
    public class GameManager : SerializeSingleton<GameManager>
    {
        [SerializeField] private InGameLevelData levelGenerationData;
        [SerializeField] private HUDController hudController;
        [SerializeField] private LevelGenerator levelGenerator;

        public int currentLevel;
        private LevelGenData currentLevelGenData;

        private void Awake()
        {
            Application.targetFrameRate = 120;

            //Fetch the level, Update current level

            //Fetch that particular levelGendata
            currentLevelGenData = levelGenerationData.Data[currentLevel];

            //Generate level - either random or level josn
            if (currentLevelGenData.generateRandomLevel)
                levelGenerator?.GenerateRandomLevel(currentLevelGenData.startSpawnPosition.x, currentLevelGenData.startSpawnPosition.y, currentLevelGenData.numberOfRows, currentLevelGenData.numberOfColumns);
            else
                levelGenerator?.GenerateLevelFromLevelJson(currentLevelGenData.levelJson);
        }
    }
}