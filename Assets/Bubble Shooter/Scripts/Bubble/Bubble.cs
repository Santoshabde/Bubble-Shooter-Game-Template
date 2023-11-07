using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using NaughtyAttributes;
using SNGames.CommonModule;
using System;

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
        [SerializeField] protected string audioIDToPlayOnFinalPosSettling;
        [SerializeField] protected TrailRenderer lineRenderer;

        public List<NeighbourData> NeighbourBubbles => neighbourBubbles;
        public BubbleType BubbleColor => bubbleColor;
        public Vector3 PositionID => positionID;
        public GameObject BubbleMesh => bubbleMesh;

        //Set a unique poisition id. PositionId is the heart of this game. All calculations are based on this.
        public void SetPositionID(Vector3 poitionID)
        {
            this.positionID = poitionID;
        }

        //set Neighbours of this bubble
        public void SetNeighbourBubblesData(List<NeighbourData> neighbourBubbles)
        {
            this.neighbourBubbles = neighbourBubbles;
        }

        //Get all neighbour points. This function returns all possible neighbour points, even if it occupied by a bubble
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

        //Moves the bubble to final position of raycast hit and then performs neccesarry actions based on the bubble type
        public void MoveLaunchBubbleToFinalPositionOnBoard(Vector3 finalPoint, Bubble bubbleWeAreShootingTo = null)
        {
            if(isLaunchBubble)
            {
                isLaunchBubble = false;

                if (lineRenderer != null)
                    lineRenderer.gameObject.SetActive(true);

                InitOnExecutingLaunch();

                StartCoroutine(MoveBallToFinalPosition(finalPoint));

                IEnumerator MoveBallToFinalPosition(Vector3 finalPoint)
                {
                    transform.DOMove(finalPoint, 0.2f);

                    yield return new WaitForSeconds(0.2f);

                    if (bubbleWeAreShootingTo != null)
                    {
                        if (AudioManager.Instance != null && !string.IsNullOrEmpty(audioIDToPlayOnFinalPosSettling))
                            AudioManager.Instance.PlayAudioClipWithAutoDestroy(audioIDToPlayOnFinalPosSettling);

                        OnLaunchBallSettleAtFinalPosition(finalPoint, bubbleWeAreShootingTo);
                    }
                }
            }
        }

        //Impact motion of bubble
        public void PlayImpactMotionAnimationForBubble(Vector3 directionOfImpact)
        {
            Sequence impactMotionSequecne = DOTween.Sequence();
            impactMotionSequecne.Append(transform.DOMove(transform.position + (directionOfImpact * 0.05f), 0.1f));
         
            impactMotionSequecne.Append(transform.DOMove(transform.position, 0.1f));
        }

        public void FreeBubbleFromTheGrid()
        {
            gameObject.AddComponent<Rigidbody2D>();
            LevelData.bubblesLevelDataDictionary.Remove(PositionID);
            Destroy(this.gameObject, 3f);
        }

        //Overide this and write the fcuntionality based on the bubble type
        protected abstract void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint, Bubble bubbleWeAreShootingTo);

        //Vfx played on deactivating this bubble
        public abstract void ActivateDeactivatedVFX();

        protected virtual void InitOnExecutingLaunch() 
        {

        }
    }
}