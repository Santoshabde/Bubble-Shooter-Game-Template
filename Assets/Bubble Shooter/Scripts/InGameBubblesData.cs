using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BubblesData", order = 1)]
public class InGameBubblesData : ScriptableObject
{
    [SerializeField] private List<Bubble> bubblePrefabsInGame;

    private Dictionary<BubbleColor, Bubble> bubblePrefabsData = null;

    public Dictionary<BubbleColor, Bubble> BubblePrefabsData
    {
        get
        {
            if (bubblePrefabsData == null)
            {
                bubblePrefabsData = new Dictionary<BubbleColor, Bubble>();
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

        BubbleColor randomColor = BubbleShooter_HelperFunctions.GiveRandomBubbleColor();
        if (BubblePrefabsData.ContainsKey(randomColor))
            randomBubbleColor = BubblePrefabsData[randomColor];

        return randomBubbleColor;
    }
}
