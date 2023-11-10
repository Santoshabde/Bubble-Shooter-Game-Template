using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

namespace SNGames.BubbleShooter
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BubblesData", order = 1)]
    public class InGameBubblesData : BaseNonKeyValueConfig<BubblesData>
    {
        //Bubble Prefabs Data
        private static Dictionary<BubbleType, Bubble> bubblePrefabsData = null;
        public static Dictionary<BubbleType, Bubble> BubblePrefabsData
        {
            get
            {
                if (bubblePrefabsData == null)
                {
                    bubblePrefabsData = new Dictionary<BubbleType, Bubble>();
                    foreach (var bubblePrefab in Data.bubblePrefabsInGame)
                    {
                        bubblePrefabsData.Add(bubblePrefab.BubbleColor, bubblePrefab);
                    }
                }

                return bubblePrefabsData;
            }
        }

        //Bubble ID And Sprite Data
        private Dictionary<BubbleType, Sprite> bubbleIdAndSprite = null;
        public Dictionary<BubbleType, Sprite> BubbleIdAndSprite
        {
            get
            {
                if (bubbleIdAndSprite == null)
                {
                    bubbleIdAndSprite = new Dictionary<BubbleType, Sprite>();
                    foreach (var bubblePrefab in Data.idAndSprite)
                    {
                        bubbleIdAndSprite.Add(bubblePrefab.bubbleType, bubblePrefab.sprite);
                    }
                }

                return bubbleIdAndSprite;
            }
        }


        //Helper functions for this config
        public static Bubble GetRandomBubbleColorPrefab()
        {
            Bubble randomBubbleColor = null;

            BubbleType randomColor = BubbleShooter_HelperFunctions.GiveRandomBubbleColor();
            if (BubblePrefabsData.ContainsKey(randomColor))
                randomBubbleColor = BubblePrefabsData[randomColor];

            return randomBubbleColor;
        }

        public static Bubble GetBubbleOfAColor(BubbleType bubbleType)
        {
            Bubble result = null;
            result = Data.bubblePrefabsInGame.Find(t => t.BubbleColor == bubbleType);
            return result;
        }
    }

    [System.Serializable]
    public class BubblesData
    {
        public List<Bubble> bubblePrefabsInGame;
        public List<BubbleIdAndSprite> idAndSprite;
        public int scorePerBubble;
    }

    [System.Serializable]
    public class BubbleIdAndSprite
    {
        public BubbleType bubbleType;
        public Sprite sprite;
    }
}
