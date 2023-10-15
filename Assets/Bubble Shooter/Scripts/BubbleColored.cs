using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SNGames.BubbleShooter;
using SNGames.CommonModule;
using NaughtyAttributes;

namespace SNGames.BubbleShooter
{
    public class BubbleColored : Bubble
    {
        [BoxGroup("VFX Data")]
        [SerializeField] protected ParticleSystem ringEffect;
        [BoxGroup("VFX Data")]
        [SerializeField] protected ParticleSystem splashEffect;

        protected override void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint)
        {
            StartCoroutine(OnLaunchBallSettleAtFinalPosition_IEnum(finalPoint));
        }

        private IEnumerator OnLaunchBallSettleAtFinalPosition_IEnum(Vector3 finalPoint)
        {
            //Set position ID of this launch bubble
            SetPositionID(finalPoint);

            //Set the layer back to default - like rest of the bubbles on board
            gameObject.layer = LayerMask.NameToLayer("Default");

            //Updating Level Data, as new bubble got added
            LevelData.bubblesLevelDataDictionary.Add(finalPoint, this);

            //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);

            //Next main step - identify same color bubbles in chain, if >=3 remove them from board
            List<Bubble> chainSameColorBubbles = BubbleShooter_HelperFunctions.GetAllReachableNodesOfAColor(this);
            List<Bubble> cachedBubblesToDeactivate = new List<Bubble>();
            if (chainSameColorBubbles.Count >= 3)
            {
                foreach (var sameColoredBubble in chainSameColorBubbles)
                {
                    cachedBubblesToDeactivate.Add(sameColoredBubble);
                    ((BubbleColored)sameColoredBubble).ActivateDeactivatedVFX();
                    LevelData.bubblesLevelDataDictionary.Remove(sameColoredBubble.PositionID);
                    yield return new WaitForSeconds(0.1f);
                }

                //Recalculating neighbour Data
                BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);
            }

            //Once bubble clears the similar colors, we need to seperate isolated bubbles from the level
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnBubbleCollisionClearDataComplete);

            yield return new WaitForSeconds(0.2f);
            cachedBubblesToDeactivate.ForEach(t => t.gameObject.SetActive(false));
        }

        public void ActivateDeactivatedVFX()
        {
            bubbleMesh.SetActive(false);
            ringEffect.gameObject.SetActive(true);
            splashEffect.gameObject.SetActive(true);
        }
    }
}