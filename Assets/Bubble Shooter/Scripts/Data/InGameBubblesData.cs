using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BubblesData", order = 1)]
    public class InGameBubblesData : ScriptableObject
    {
        [SerializeField] private List<Bubble> bubblePrefabsInGame;
        [SerializeField] private int scorePerBubble;

        private Dictionary<BubbleType, Bubble> bubblePrefabsData = null;

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
}
