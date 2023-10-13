using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace SNGames.BubbleShooter
{
    public class LevelGenerator : MonoBehaviour
    {
        public static float bubbleGap = 0.5f;

        #region Testing Only
        [SerializedDictionary]
        public SerializedDictionary<Vector3, Bubble> bubblesLevelData = new SerializedDictionary<Vector3, Bubble>();
        #endregion

        [SerializeField] private InGameBubblesData inGameBubblesData;
        [SerializeField] private float startX;
        [SerializeField] private float startY;
        [SerializeField] private int initialNumberOfRows;
        [SerializeField] private int initialNumberOfColumns;

        private void Start()
        {
            //Generates Level - All bubbles - And Fill up the level data
            GenerateLevel(startX, startY, initialNumberOfRows, initialNumberOfColumns);

            //Calculates all neighbours 
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelData, bubbleGap);
        }

        #region Testing Only
        private void Update()
        {
            bubblesLevelData = new SerializedDictionary<Vector3, Bubble>();
            foreach (var item in LevelData.bubblesLevelData)
            {
                bubblesLevelData.Add(item.Key, item.Value);
            }
        }
        #endregion

        private void GenerateLevel(float startX, float startY, int rows, int columns)
        {
            LevelData.bubblesLevelData = new Dictionary<Vector3, Bubble>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //Choosing Random Color bubble
                    Bubble bubbleChoosen = inGameBubblesData.GetRandomBubbleColorPrefab();

                    float xOffset = 0;
                    if (i % 2 == 1)
                        xOffset = bubbleGap / 2;

                    Vector3 positionBubbleShouldSpawn = new Vector3(startX + (j * bubbleGap) + xOffset, startY + (i * bubbleGap), 0);
                    Bubble instantiatedBubble = Instantiate(bubbleChoosen, positionBubbleShouldSpawn, Quaternion.identity);
                    instantiatedBubble.SetPositionID(positionBubbleShouldSpawn);
                    instantiatedBubble.transform.SetParent(transform);
                    LevelData.bubblesLevelData.Add(positionBubbleShouldSpawn, instantiatedBubble);
                }
            }
        }
    }
}
