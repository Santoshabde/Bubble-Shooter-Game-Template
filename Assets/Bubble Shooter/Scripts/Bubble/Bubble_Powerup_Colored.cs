using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SNGames.BubbleShooter
{
    public class Bubble_Powerup_Colored : Bubble
    {
        [Header("Multi Colored Powerup Data")]
        [SerializeField] private ParticleSystem initialGlow;
        [SerializeField] private GameObject parentMultiColorObject;
        [SerializeField] private List<IdAndGameObject> bubbleIdAndColor;

        protected override void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint, Bubble bubbleWeAreShootingTo)
        {
            StartCoroutine(OnLaunchBallSettleAtFinalPosition_IEnum(finalPoint, bubbleWeAreShootingTo));
        }

        private IEnumerator OnLaunchBallSettleAtFinalPosition_IEnum(Vector3 finalPoint, Bubble bubbleWeAreShootingTo)
        {
            //Set position ID of this launch bubble
            SetPositionID(finalPoint);

            //Set the layer back to default - like rest of the bubbles on board
            gameObject.layer = LayerMask.NameToLayer("Default");
            if (lineRenderer != null)
                lineRenderer.gameObject.SetActive(false);

            //Change color to attached bubble which you aimed to
            if (bubbleWeAreShootingTo.BubbleColor == BubbleType.NonDestructable)
                bubbleColor = BubbleShooter_HelperFunctions.GiveRandomBubbleColor();
            else
                bubbleColor = bubbleWeAreShootingTo.BubbleColor;

            UpdateToRespectiveColorMesh(bubbleColor);

            //Updating Level Data, as new bubble got added
            LevelData.bubblesLevelDataDictionary.Add(finalPoint, this);

            //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);

            //Giving a impact animation for all neighbouring bubbles
            foreach (var neighbourData in neighbourBubbles)
            {
                (neighbourData.bubble).PlayImpactMotionAnimationForBubble((neighbourData.bubble.transform.position - transform.position).normalized);
            }

            yield return new WaitForSeconds(0.3f);
            //Next main step - identify same color bubbles in chain, if >=3 remove them from board
            List<Bubble> chainSameColorBubbles = BubbleShooter_HelperFunctions.GetAllReachableNodesOfAColor(this);
            List<Bubble> cachedBubblesToDeactivate = new List<Bubble>();
            if (chainSameColorBubbles.Count >= 3)
            {
                foreach (var sameColoredBubble in chainSameColorBubbles)
                {
                    cachedBubblesToDeactivate.Add(sameColoredBubble);
                    sameColoredBubble.ActivateDeactivatedVFX();
                    LevelData.bubblesLevelDataDictionary.Remove(sameColoredBubble.PositionID);

                    yield return new WaitForSeconds(0.1f);
                }

                //Update Target Score data
                GameManager.Instance.UpdateGameTargetsScore(chainSameColorBubbles);

                //Recalculating neighbour Data
                BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);
            }

            //Once bubble clears the similar colors, we need to seperate isolated bubbles from the level
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnBubbleCollisionClearDataComplete);

            yield return new WaitForSeconds(0.2f);
            cachedBubblesToDeactivate.ForEach(t => t.gameObject.SetActive(false));

            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.MoveNextBubbleToCurrentBubble);
        }

        public override void ActivateDeactivatedVFX()
        {
            parentMultiColorObject.SetActive(false);
            BubbleMesh.SetActive(false);
        }

        protected override void InitOnExecutingLaunch()
        {
            initialGlow.gameObject.SetActive(false);
        }

        private void UpdateToRespectiveColorMesh(BubbleType colorToUpdateTo)
        {
            foreach (var item in bubbleIdAndColor)
            {
                if(item.bubbleColor == colorToUpdateTo)
                {
                    BubbleMesh.SetActive(false);
                    item.bubble.SetActive(true);
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class IdAndGameObject
    {
        public BubbleType bubbleColor;
        public GameObject bubble;
    }
}
