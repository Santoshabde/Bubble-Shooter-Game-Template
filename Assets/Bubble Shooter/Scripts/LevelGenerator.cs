using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using SNGames.CommonModule;
using System.Linq;

namespace SNGames.BubbleShooter
{
    public class LevelGenerator : MonoBehaviour
    {
        public static float bubbleGap = 0.5f;

        [SerializeField] private InGameBubblesData inGameBubblesData;
        [SerializeField] private float startX;
        [SerializeField] private float startY;
        [SerializeField] private int initialNumberOfRows;
        [SerializeField] private int initialNumberOfColumns;

        private void Start()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnBubbleCollisionClearDataComplete, ClearTheIsolatedBubblesInLevel);

            //Generates Level - All bubbles - And Fill up the level data
            GenerateLevel(startX, startY, initialNumberOfRows, initialNumberOfColumns);

            //Calculates all neighbours 
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, bubbleGap);
        }

        private void GenerateLevel(float startX, float startY, int rows, int columns)
        {
            LevelData.bubblesLevelDataDictionary = new Dictionary<Vector3, Bubble>();
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
                    LevelData.bubblesLevelDataDictionary.Add(positionBubbleShouldSpawn, instantiatedBubble);
                }
            }
        }

        private void ClearTheIsolatedBubblesInLevel()
        {
            Bubble nodeBubbleToCalculateBFS = LevelData.bubblesLevelDataDictionary[new Vector3(-2.5f, 5.5f, 0)];
            List<Bubble> allNodes;
            List<Bubble> allVisitedNodes;
            List<Bubble> leftOutNodes;

            allNodes = new List<Bubble>();
            foreach (var item in LevelData.bubblesLevelDataDictionary)
            {
                allNodes.Add(item.Value);
            }

            allVisitedNodes = BubbleShooter_HelperFunctions.GetAllReachableNodesOfAnyColor(nodeBubbleToCalculateBFS);

            leftOutNodes = new List<Bubble>();
            leftOutNodes = allNodes.Except(allVisitedNodes).ToList();

            foreach (var item in leftOutNodes)
            {
                item.gameObject.AddComponent<Rigidbody2D>();
                LevelData.bubblesLevelDataDictionary.Remove(item.PositionID);
            }

            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);
        }
    }
}
