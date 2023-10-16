using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class BubbleShooter_HelperFunctions
    {
        /// <summary>
        /// Get a random bubble color
        /// </summary>
        /// <returns></returns>
        public static BubbleType GiveRandomBubbleColor()
        {
            BubbleType randomColor = BubbleType.Green;

            int random = Random.Range(0, 5);
            if (random == 0)
                randomColor = BubbleType.Green;

            if (random == 1)
                randomColor = BubbleType.Pink;

            if (random == 2)
                randomColor = BubbleType.Red;

            if (random == 3)
                randomColor = BubbleType.White;

            if (random == 4)
                randomColor = BubbleType.Yellow;

            return randomColor;
        }

        /// <summary>
        /// Each bubble has 6 neighbour points in the game, so this function returns the nearest neighbour point to the given point(passed as function parameter)
        /// </summary>
        /// <param name="bubble">Bubble on which this function is executed</param>
        /// <param name="point">Point on which the nearest neighbour point is calculated</param>
        /// <returns></returns>
        public static Vector3 GetNearestNeighbourBubblePoint(Bubble bubble, Vector3 point)
        {
            Vector3 nearestPoint = Vector3.zero;

            float nearestDistance = Mathf.Infinity;
            foreach (var item in bubble.GetAllPositionNeighbourPoints())
            {
                if (Vector3.Distance(item, point) < nearestDistance
                    && !LevelData.bubblesLevelDataDictionary.ContainsKey(item))
                {
                    nearestDistance = Vector3.Distance(item, point);
                    nearestPoint = item;
                }
            }

            return nearestPoint;
        }

        /// <summary>
        /// Recaculates neighbour data for all bubble in game
        /// </summary>
        public static void RecalculateAllBubblesNeighboursData(Dictionary<Vector3, Bubble> bubblesLevelData, float bubbleGap)
        {
            foreach (var bubbleData in bubblesLevelData)
            {
                List<NeighbourData> neighbourBubbles = new List<NeighbourData>();

                //Current Bubble Position
                Vector3 bubblePosition = bubbleData.Key;

                //Right    
                Vector3 bubbleRightPosition = bubblePosition + new Vector3(bubbleGap, 0, 0);
                if (bubblesLevelData.ContainsKey(bubbleRightPosition))
                {
                    NeighbourData rightNeighbourData = new NeighbourData();

                    rightNeighbourData.bubble = bubblesLevelData[bubbleRightPosition];
                    rightNeighbourData.direction = NeighbourDirection.Right;

                    neighbourBubbles.Add(rightNeighbourData);
                }

                //Left    
                Vector3 bubbleLeftPosition = bubblePosition + new Vector3(-bubbleGap, 0, 0);
                if (bubblesLevelData.ContainsKey(bubbleLeftPosition))
                {
                    NeighbourData leftNeighbourData = new NeighbourData();

                    leftNeighbourData.bubble = bubblesLevelData[bubbleLeftPosition];
                    leftNeighbourData.direction = NeighbourDirection.Left;

                    neighbourBubbles.Add(leftNeighbourData);
                }

                //Top Right    
                Vector3 bubbleTopRightPosition = bubblePosition + new Vector3(bubbleGap / 2, bubbleGap, 0);
                if (bubblesLevelData.ContainsKey(bubbleTopRightPosition))
                {
                    NeighbourData topRightNeighbourData = new NeighbourData();

                    topRightNeighbourData.bubble = bubblesLevelData[bubbleTopRightPosition];
                    topRightNeighbourData.direction = NeighbourDirection.TopRight;

                    neighbourBubbles.Add(topRightNeighbourData);
                }

                //Top Left    
                Vector3 bubbleTopLeftPosition = bubblePosition + new Vector3(-(bubbleGap / 2), bubbleGap, 0);
                if (bubblesLevelData.ContainsKey(bubbleTopLeftPosition))
                {
                    NeighbourData topLeftNeighbourData = new NeighbourData();

                    topLeftNeighbourData.bubble = bubblesLevelData[bubbleTopLeftPosition];
                    topLeftNeighbourData.direction = NeighbourDirection.TopLeft;

                    neighbourBubbles.Add(topLeftNeighbourData);
                }

                //Bottom Left    
                Vector3 bubbleBottomLeftPosition = bubblePosition + new Vector3(-(bubbleGap / 2), -bubbleGap, 0);
                if (bubblesLevelData.ContainsKey(bubbleBottomLeftPosition))
                {
                    NeighbourData bottomLeftNeighbourData = new NeighbourData();

                    bottomLeftNeighbourData.bubble = bubblesLevelData[bubbleBottomLeftPosition];
                    bottomLeftNeighbourData.direction = NeighbourDirection.BottomLeft;

                    neighbourBubbles.Add(bottomLeftNeighbourData);
                }

                //Bottom Right    
                Vector3 bubbleBottomRightPosition = bubblePosition + new Vector3((bubbleGap / 2), -bubbleGap, 0);
                if (bubblesLevelData.ContainsKey(bubbleBottomRightPosition))
                {
                    NeighbourData bottomRightNeighbourData = new NeighbourData();

                    bottomRightNeighbourData.bubble = bubblesLevelData[bubbleBottomRightPosition];
                    bottomRightNeighbourData.direction = NeighbourDirection.BottomRight;

                    neighbourBubbles.Add(bottomRightNeighbourData);
                }

                //Setting neighbour Data
                bubbleData.Value.SetNeighbourBubblesData(neighbourBubbles);
            }
        }

        /// <summary>
        /// Uses BFS algorithm to get all connecting chain bubbles of the 'startingBubble' color
        /// </summary>
        /// <param name="startingBubble"></param>
        /// <returns></returns>
        public static List<Bubble> GetAllReachableNodesOfAColor(Bubble startingBubble)
        {
            List<Bubble> resultBubbles = new List<Bubble>();

            List<Bubble> visitedBubbles = new List<Bubble>();

            Queue<Bubble> queue = new Queue<Bubble>();
            queue.Enqueue(startingBubble);
            visitedBubbles.Add(startingBubble);

            while (queue.Count != 0)
            {
                Bubble poppedBubble = queue.Dequeue();
                resultBubbles.Add(poppedBubble);

                foreach (var item in poppedBubble.NeighbourBubbles)
                {
                    if (item.bubble.BubbleColor == startingBubble.BubbleColor
                        && (!visitedBubbles.Contains(item.bubble)))
                    {
                        queue.Enqueue(item.bubble);
                        visitedBubbles.Add(item.bubble);
                    }
                }
            }

            return resultBubbles;
        }

        public static List<Bubble> GetAllReachableNodesOfAnyColor(Bubble startingBubble)
        {
            List<Bubble> visitedBubbles = new List<Bubble>();
            Queue<Bubble> queue = new Queue<Bubble>();

            queue.Enqueue(startingBubble);
            visitedBubbles.Add(startingBubble);

            while (queue.Count != 0)
            {
                Bubble poppedBubble = queue.Dequeue();

                foreach (var item in poppedBubble.NeighbourBubbles)
                {
                    if ((!visitedBubbles.Contains(item.bubble)))
                    {
                        queue.Enqueue(item.bubble);
                        visitedBubbles.Add(item.bubble);
                    }
                }
            }

            return visitedBubbles;
        }

        public static List<Bubble> GetExploredBubblesOfCertainLevel(Bubble startingBubble, int level)
        {
            List<Bubble> visitedBubbles = new List<Bubble>();
            Queue<Bubble> queue = new Queue<Bubble>();

            queue.Enqueue(startingBubble);
            visitedBubbles.Add(startingBubble);

            int levelExplored = 0;

            while (queue.Count != 0)
            {
                levelExplored += 1;
                Bubble poppedBubble = queue.Dequeue();

                foreach (var item in poppedBubble.NeighbourBubbles)
                {
                    if ((!visitedBubbles.Contains(item.bubble)))
                    {
                        if (levelExplored <= level)
                            queue.Enqueue(item.bubble);

                        visitedBubbles.Add(item.bubble);
                    }
                }
            }

            return visitedBubbles;
        }
    }
}