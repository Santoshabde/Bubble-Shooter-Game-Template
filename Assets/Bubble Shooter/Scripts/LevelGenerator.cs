using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using SNGames.CommonModule;
using System.Linq;
using DG.Tweening;

namespace SNGames.BubbleShooter
{
    public class LevelGenerator : BaseService
    {
        public static float bubbleGap = 0.5f;

        private Bubble nodeBubbleToCalculateBFS;
        private List<Bubble> allBubblesSpawnedInTheLevel = new List<Bubble>();

        public override void Init()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnBubbleCollisionClearDataComplete, ClearTheIsolatedBubblesInLevel);

            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnLevelSuccess, ClearCurrentLevel);
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnLevelFail, ClearCurrentLevel);
        }

        public override void Deinit()
        {
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnBubbleCollisionClearDataComplete, ClearTheIsolatedBubblesInLevel);

            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnLevelSuccess, ClearCurrentLevel);
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnLevelFail, ClearCurrentLevel);
        }

        #region Level Generation Types

        public void GenerateRandomLevel(float startX, float startY, int rows, int columns, bool shouldAnimateWhileSpawning = false)
        {
            LevelData.bubblesLevelDataDictionary = new Dictionary<Vector3, Bubble>();
            for (int i = 0; i < rows; i++)
            {
                float xOffset = 0;
                if (i % 2 == 1)
                {
                    xOffset = bubbleGap / 2;
                    for (int j = 0; j <= columns; j++)
                    {
                        //Choosing Random Color bubble
                        Bubble bubbleChoosen = InGameBubblesData.GetRandomBubbleColorPrefab();

                        Vector3 positionBubbleShouldSpawn = new Vector3(startX + (j * bubbleGap) - xOffset, startY + (i * bubbleGap), 0);
                        Bubble instantiatedBubble = GameObject.Instantiate(bubbleChoosen, shouldAnimateWhileSpawning ? new Vector3(0, -5, 0) : positionBubbleShouldSpawn, Quaternion.identity);
                        instantiatedBubble.SetPositionID(positionBubbleShouldSpawn);
                        //instantiatedBubble.transform.SetParent(transform);

                        LevelData.bubblesLevelDataDictionary.Add(positionBubbleShouldSpawn, instantiatedBubble);
                        allBubblesSpawnedInTheLevel.Add(instantiatedBubble);
                    }
                }
                else
                {
                    for (int j = 0; j < columns; j++)
                    {
                        //Choosing Random Color bubble
                        Bubble bubbleChoosen = InGameBubblesData.GetRandomBubbleColorPrefab();

                        Vector3 positionBubbleShouldSpawn = new Vector3(startX + (j * bubbleGap) + xOffset, startY + (i * bubbleGap), 0);
                        Bubble instantiatedBubble = GameObject.Instantiate(bubbleChoosen, shouldAnimateWhileSpawning ? new Vector3(0, -5, 0) : positionBubbleShouldSpawn, Quaternion.identity);
                        instantiatedBubble.SetPositionID(positionBubbleShouldSpawn);
                        //instantiatedBubble.transform.SetParent(transform);

                        LevelData.bubblesLevelDataDictionary.Add(positionBubbleShouldSpawn, instantiatedBubble);
                        allBubblesSpawnedInTheLevel.Add(instantiatedBubble);
                    }
                }
            }

            if (shouldAnimateWhileSpawning)
                AnimateBubblesToTheirPositions();

            //Spawn a indestructable bubble row as final row
            Bubble indestructableBubblePrefab = InGameBubblesData.GetBubbleOfAColor(BubbleType.NonDestructable);
            for (int i = 0; i < columns; i++)
            {
                float xOffset = 0;
                if(rows % 2 != 0)
                    xOffset = xOffset = bubbleGap / 2;

                Vector3 positionBubbleShouldSpawn = new Vector3(startX + (i * bubbleGap) + xOffset, startY + (rows * bubbleGap), 0);
                Bubble instantiatedBubble = GameObject.Instantiate(indestructableBubblePrefab, positionBubbleShouldSpawn, Quaternion.identity);
                instantiatedBubble.SetPositionID(positionBubbleShouldSpawn);
               // instantiatedBubble.transform.SetParent(transform);

                LevelData.bubblesLevelDataDictionary.Add(positionBubbleShouldSpawn, instantiatedBubble);
                allBubblesSpawnedInTheLevel.Add(instantiatedBubble);

                //NOTE!!!!! Choosing nodeBubbleToCalculateBFS is important to set up - its used for multiple path finding algos to find out isolated bubbles in the game
                nodeBubbleToCalculateBFS = instantiatedBubble;
            }

            //Calculates all neighbours 
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, bubbleGap);
        }

        public void GenerateLevelFromLevelJson(string levelJson)
        {
            LevelData.bubblesLevelDataDictionary = new Dictionary<Vector3, Bubble>();

            BubbleLevelJson bubbleLevelDataToSpawn = JsonUtility.FromJson<BubbleLevelJson>(levelJson);
            List<BubblePositionID> bubbles = bubbleLevelDataToSpawn.bubbles;

            foreach (var bubbleInfo in bubbles)
            {
                Bubble bubbleChoosen = InGameBubblesData.GetBubbleOfAColor(bubbleInfo.bubbleType);
                Vector3 positionBubbleShouldSpawn = bubbleInfo.bubbleSpawnedPosition;

                Bubble instantiatedBubble = GameObject.Instantiate(bubbleChoosen, positionBubbleShouldSpawn, Quaternion.identity);
                instantiatedBubble.SetPositionID(positionBubbleShouldSpawn);
                //instantiatedBubble.transform.SetParent(transform);

                LevelData.bubblesLevelDataDictionary.Add(positionBubbleShouldSpawn, instantiatedBubble);
                allBubblesSpawnedInTheLevel.Add(instantiatedBubble);

                //NOTE!!!!! Choosing nodeBubbleToCalculateBFS is important to set up - its used for multiple path finding algos to find out isolated bubbles in the game
                if (instantiatedBubble.BubbleColor == BubbleType.NonDestructable)
                    nodeBubbleToCalculateBFS = instantiatedBubble;
            }

            //Calculates all neighbours 
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, bubbleGap);
        }

        private void AnimateBubblesToTheirPositions()
        {
            foreach (var bubbleData in LevelData.bubblesLevelDataDictionary)
            {
                bubbleData.Value.transform.DOMove(bubbleData.Key, 0.8f);
            }
        }

        #endregion

        private void ClearTheIsolatedBubblesInLevel()
        {
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
                item.FreeBubbleFromTheGrid();
            }

            GameManager.Instance.UpdateGameTargetsScore(leftOutNodes);

            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);
        }

        private void ClearCurrentLevel()
        {
            foreach (var item in allBubblesSpawnedInTheLevel)
            {
                if (item != null)
                    GameObject.Destroy(item.gameObject);
            }

            allBubblesSpawnedInTheLevel.Clear();
            allBubblesSpawnedInTheLevel = new List<Bubble>();

            LevelData.ResetLevelData();
        }

        public Bubble GetNearestRowBubbleInTheGrid(Transform refPoint)
        {
            Bubble minDistanceBubble = null;

            float minDistance = 100;
            foreach (var item in LevelData.bubblesLevelDataDictionary)
            {
                float distance = Vector3.Distance(item.Key, refPoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceBubble = item.Value;
                }
            }

            return minDistanceBubble;
        }
    }
}
