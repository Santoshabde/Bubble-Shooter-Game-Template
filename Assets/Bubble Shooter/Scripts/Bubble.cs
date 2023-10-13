using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Bubble : MonoBehaviour
{
    //Only for viewning and Debug
    [SerializeField] private Vector3 positionID;
    [SerializeField] private List<NeighbourData> neighbourBubbles;

    //Need to be filled by user
    [SerializeField] private BubbleColor bubbleColor;

    public List<NeighbourData> NeighbourBubbles => neighbourBubbles;
    public BubbleColor BubbleColor => bubbleColor;
    public Vector3 PositionID => positionID;

    public bool isLaunchBubble = false;

    public void SetPositionID(Vector3 poitionID)
    {
        this.positionID = poitionID;
    }

    public void SetNeighbourBubblesData(List<NeighbourData> neighbourBubbles)
    {
        this.neighbourBubbles = neighbourBubbles;
    }

    public List<Vector3> GetAllPositionNeighbourPoints()
    {
        List<Vector3> possibleNeighbourPoints = new List<Vector3>();

        possibleNeighbourPoints.Add(positionID + new Vector3(LevelGenerator.bubbleGap, 0, 0));
        possibleNeighbourPoints.Add(positionID + new Vector3(-LevelGenerator.bubbleGap, 0, 0));
        possibleNeighbourPoints.Add(positionID + new Vector3(LevelGenerator.bubbleGap / 2, LevelGenerator.bubbleGap, 0));
        possibleNeighbourPoints.Add(positionID + new Vector3(-LevelGenerator.bubbleGap / 2, LevelGenerator.bubbleGap, 0));
        possibleNeighbourPoints.Add(positionID + new Vector3(LevelGenerator.bubbleGap / 2, -LevelGenerator.bubbleGap, 0));
        possibleNeighbourPoints.Add(positionID + new Vector3(-LevelGenerator.bubbleGap / 2, -LevelGenerator.bubbleGap, 0));

        return possibleNeighbourPoints;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bubble collidedBubble;
        if (collision.TryGetComponent<Bubble>(out collidedBubble))
        {
            if (!isLaunchBubble)
                return;

            isLaunchBubble = false;

            //Disable Rigid body and stop it
            transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            Destroy(transform.GetComponent<Rigidbody2D>());

            //Calculate nearest Bubble and setting this bubble there
            Vector3 pointToMoveTo = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(collidedBubble, transform.position);
            SetPositionID(pointToMoveTo);
            transform.DOMove(pointToMoveTo, 0.2f);

            //Updating Level Data, as new bubble got added
            LevelData.bubblesLevelData.Add(pointToMoveTo, this);

            //Recalculating neighbour Data
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelData, LevelGenerator.bubbleGap);

            StartCoroutine(CheckIfAnyOfTheNeighbourBubbleIsSameColor());
        }
    }

    private IEnumerator CheckIfAnyOfTheNeighbourBubbleIsSameColor()
    {
        yield return new WaitForSeconds(0.5f);

        List<Bubble> chainSameColorBubbles = BubbleShooter_HelperFunctions.GetAllChainBubblesNeighbourOfAColor(this);

        if (chainSameColorBubbles.Count >= 3)
        {
            foreach (var item in chainSameColorBubbles)
            {
                LevelData.bubblesLevelData.Remove(item.positionID);
                item.gameObject.SetActive(false);
            }

            //Recalculating neighbour Data
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelData, LevelGenerator.bubbleGap);
        }
    }
}

