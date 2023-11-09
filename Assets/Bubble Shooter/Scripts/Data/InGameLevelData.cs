using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.CommonModule;

namespace SNGames.BubbleShooter
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class InGameLevelData : BaseKeyValueConfig<LevelGenData>
    {

    }

    #region Data structures

    [System.Serializable]
    public class LevelGenData : IKeyValueConfigData
    {
        public int levelNumber;
        public bool generateRandomLevel;
        public Vector2 startSpawnPosition;
        public int numberOfRows;
        public int numberOfColumns;
        public string levelJson;
        public List<TargetLevelBubble> targetBubbles;
        public List<BubbleType> bubblesToShootOrder;
        public int targetRacoonsToSave;
        public int totalGameTimeInSeconds;

        public string ID => levelNumber.ToString();
    }

    [System.Serializable]
    public class TargetLevelBubble
    {
        public int targetNumber;
        public BubbleType targetBubble;
    }

    #endregion
}