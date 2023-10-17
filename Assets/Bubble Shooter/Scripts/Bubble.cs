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
        [Header("Base Bubble Data")]
        [SerializeField] protected BubbleType bubbleColor;
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

        public void MoveLaunchBubbleToFinalPositionOnBoard(Vector3 finalPoint, Bubble bubbleWeAreShootingTo)
        {
            if(isLaunchBubble)
            {
                isLaunchBubble = false;

                StartCoroutine(MoveBallToFinalPosition(finalPoint));

                IEnumerator MoveBallToFinalPosition(Vector3 finalPoint)
                {
                    transform.DOMove(finalPoint, 0.2f);

                    yield return new WaitForSeconds(0.2f);

                    OnLaunchBallSettleAtFinalPosition(finalPoint, bubbleWeAreShootingTo);
                }
            }
        }

        public void PlayImpactMotionAnimationForBubble(Vector3 directionOfImpact)
        {
            Sequence impactMotionSequecne = DOTween.Sequence();
            impactMotionSequecne.Append(transform.DOMove(transform.position + (directionOfImpact * 0.05f), 0.1f));
            impactMotionSequecne.Append(transform.DOMove(transform.position, 0.1f));
        }

        protected abstract void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint, Bubble bubbleWeAreShootingTo);

        public abstract void ActivateDeactivatedVFX();
    }
}