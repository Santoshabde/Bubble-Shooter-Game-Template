using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BubblesData", order = 1)]
    public class InGameBubblesData : ScriptableObject
    {
        [SerializeField] private List<Bubble> bubblePrefabsInGame;
        [SerializeField] private List<BubbleIdAndSprite> idAndSprite;
        [SerializeField] private int scorePerBubble;

        private Dictionary<BubbleType, Bubble> bubblePrefabsData = null;
        private Dictionary<BubbleType, Sprite> bubbleIdAndSprite = null;
        public Dictionary<BubbleType, Bubble> BubblePrefabsData
        {
            get
            {
                if (bubblePrefabsData == null)
                {
                    bubblePrefabsData = new Dictionary<BubbleType, Bubble>();
                    foreach (var bubblePrefab in bubblePrefabsInGame)
                    {
                        bubblePrefabsData.Add(bubblePrefab.BubbleColor, bubblePrefab);
                    }
                }

                return bubblePrefabsData;
            }
        }

        public Dictionary<BubbleType, Sprite> BubbleIdAndSprite
        {
            get
            {
                if (bubbleIdAndSprite == null)
                {
                    bubbleIdAndSprite = new Dictionary<BubbleType, Sprite>();
                    foreach (var bubblePrefab in idAndSprite)
                    {
                        bubbleIdAndSprite.Add(bubblePrefab.bubbleType, bubblePrefab.sprite);
                    }
                }

                return bubbleIdAndSprite;
            }
        }


        public Bubble GetRandomBubbleColorPrefab()
        {
            Bubble randomBubbleColor = null;

            BubbleType randomColor = BubbleShooter_HelperFunctions.GiveRandomBubbleColor();
            if (BubblePrefabsData.ContainsKey(randomColor))
                randomBubbleColor = BubblePrefabsData[randomColor];

            return randomBubbleColor;
        }

        public Bubble GetBubbleOfAColor(BubbleType bubbleType)
        {
            Bubble result = null;
            result = bubblePrefabsInGame.Find(t => t.BubbleColor == bubbleType);
            return result;
        }
    }

    [System.Serializable]
    public class BubbleIdAndSprite
    {
        public BubbleType bubbleType;
        public Sprite sprite;
    }
}
