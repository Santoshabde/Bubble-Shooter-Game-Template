using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SNGames.BubbleShooter;

public class BubbleColored : Bubble
{
    protected override void OnTriggerEnterWithABubble(Bubble collidedBubble)
    {
        //Disable Rigid body and stop it
        transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Destroy(transform.GetComponent<Rigidbody2D>());

        //Calculate nearest Bubble and possitioning this bubble there
        Vector3 pointToMoveTo = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(collidedBubble, transform.position);
        SetPositionID(pointToMoveTo);
        transform.DOMove(pointToMoveTo, 0.2f);

        //Updating Level Data, as new bubble got added
        LevelData.bubblesLevelData.Add(pointToMoveTo, this);

        //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
        BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelData, LevelGenerator.bubbleGap);

        StartCoroutine(CheckIfAnyOfTheNeighbourBubbleIsSameColor());

        IEnumerator CheckIfAnyOfTheNeighbourBubbleIsSameColor()
        {
            yield return new WaitForSeconds(0.5f);

            List<Bubble> chainSameColorBubbles = BubbleShooter_HelperFunctions.GetAllChainBubblesNeighbourOfAColor(this);

            if (chainSameColorBubbles.Count >= 3)
            {
                foreach (var item in chainSameColorBubbles)
                {
                    LevelData.bubblesLevelData.Remove(item.PositionID);
                    item.gameObject.SetActive(false);
                }

                //Recalculating neighbour Data
                BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelData, LevelGenerator.bubbleGap);
            }
        }
    }
}
