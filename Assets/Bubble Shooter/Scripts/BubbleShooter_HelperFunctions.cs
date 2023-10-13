using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter_HelperFunctions
{
    public static BubbleColor GiveRandomBubbleColor()
    {
        BubbleColor randomColor = BubbleColor.Green;

        int random = Random.Range(0, 5);
        if (random == 0)
            randomColor = BubbleColor.Green;

        if (random == 1)
            randomColor = BubbleColor.Pink;

        if (random == 2)
            randomColor = BubbleColor.Red;

        if (random == 3)
            randomColor = BubbleColor.White;

        if (random == 4)
            randomColor = BubbleColor.Yellow;

        return randomColor;
    }

    public static Vector3 GetNearestNeighbourBubblePoint(Bubble bubble, Vector3 point)
    {
        Vector3 nearestPoint = Vector3.zero;

        float nearestDistance = Mathf.Infinity;
        foreach (var item in bubble.GetAllPositionNeighbourPoints())
        {
            if(Vector3.Distance(item, point) < nearestDistance
                && !LevelData.bubblesLevelData.ContainsKey(item))
            {
                nearestDistance = Vector3.Distance(item, point);
                nearestPoint = item;
            }
        }

        return nearestPoint;
    }

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

    public static List<Bubble> GetAllChainBubblesNeighbourOfAColor(Bubble startingBubble)
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
                if(item.bubble.BubbleColor == startingBubble.BubbleColor
                    && (!visitedBubbles.Contains(item.bubble)))
                {
                    queue.Enqueue(item.bubble);
                    visitedBubbles.Add(item.bubble);
                }
            }
        }

        return resultBubbles;
    }
}
