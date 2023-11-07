using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    /// <summary>
    /// Commonly shared accross all classes for this particular level!!
    /// </summary>
    public static class LevelData
    {
        public static Dictionary<Vector3, Bubble> bubblesLevelDataDictionary = new Dictionary<Vector3, Bubble>();

        public static LevelGenData currentLevelGenData = null;

        public static Dictionary<BubbleType, int> currentLevelCurrentTargetStatus = new Dictionary<BubbleType, int>();

        public static void ResetLevelData()
        {
            bubblesLevelDataDictionary.Clear();
            bubblesLevelDataDictionary = new Dictionary<Vector3, Bubble>();

            currentLevelCurrentTargetStatus.Clear();
            currentLevelCurrentTargetStatus = new Dictionary<BubbleType, int>();
        }
    }
}