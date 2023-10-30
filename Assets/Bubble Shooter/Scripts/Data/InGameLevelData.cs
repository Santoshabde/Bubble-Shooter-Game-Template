using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class InGameLevelData : ScriptableObject
    {
        [SerializeField]
        private List<LevelGenData> levelGenData;

        private Dictionary<int, LevelGenData> data;

        public Dictionary<int, LevelGenData> Data
        {
            get
            {
                if(data == null)
                {
                    data = new Dictionary<int, LevelGenData>();

                    foreach (var item in levelGenData)
                    {
                        data.Add(item.levelNumber, item);
                    }
                }

                return data;
            }
        }
    }

    [System.Serializable]
    public class LevelGenData
    {
        public int levelNumber;
        public bool generateRandomLevel;
        public Vector2 startSpawnPosition;
        public int numberOfRows;
        public int numberOfColumns;
        public string levelJson;
        public List<TargetLevelBubble> targetBubbles;
        public int targetRacoonsToSave;
        public int totalGameTimeInSeconds;
    }

    [System.Serializable]
    public class TargetLevelBubble
    {
        public int targetNumber;
        public BubbleType targetBubble;
    }
}