using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using NaughtyAttributes;
using SNGames.CommonModule;

namespace SNGames.BubbleShooter
{
    public abstract class Bubble : MonoBehaviour
    {
        //Only for viewning and Debug
        [Header("Only Debug Purpose")]
        [SerializeField, ReadOnly] protected Vector3 positionID;
        [SerializeField, ReadOnly] protected List<NeighbourData> neighbourBubbles;
        [SerializeField, ReadOnly] public bool isLaunchBubble = false;

        //Need to be filled by user
        [BoxGroup("Required Data")]
        [SerializeField] protected BubbleType bubbleColor;
        [BoxGroup("Required Data")]
        [SerializeField] protected GameObject bubbleMesh;

        public List<NeighbourData> NeighbourBubbles => neighbourBubbles;
        public BubbleType BubbleColor => bubbleColor;
        public Vector3 PositionID => positionID;
        public GameObject BubbleMesh => bubbleMesh;

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

        public void MoveLaunchBubbleToFinalPositionOnBoard(Vector3 finalPoint)
        {
            if(isLaunchBubble)
            {
                isLaunchBubble = false;

                StartCoroutine(MoveBallToFinalPosition(finalPoint));

                IEnumerator MoveBallToFinalPosition(Vector3 finalPoint)
                {
                    transform.DOMove(finalPoint, 0.2f);

                    yield return new WaitForSeconds(0.2f);

                    OnLaunchBallSettleAtFinalPosition(finalPoint);
                }
            }
        }

        protected abstract void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint);
    }
}