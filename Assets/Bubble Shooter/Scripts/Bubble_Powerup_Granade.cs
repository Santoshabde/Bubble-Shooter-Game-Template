using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class Bubble_Powerup_Granade : Bubble
    {
        [SerializeField] private ParticleSystem explosionParticleEffect;

        protected override void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint)
        {
            StartCoroutine(OnLaunchBallSettleAtFinalPosition_IEnum(finalPoint));
        }

        private IEnumerator OnLaunchBallSettleAtFinalPosition_IEnum(Vector3 finalPoint)
        {
            //Set position ID of this launch bubble
            SetPositionID(finalPoint);

            //Updating Level Data, as new bubble got added
            LevelData.bubblesLevelDataDictionary.Add(finalPoint, this);

            //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);

            //Giving a impact animation for all neighbouring bubbles
            foreach (var neighbourData in neighbourBubbles)
            {
                ((BubbleColored)neighbourData.bubble).PlayImpactMotionAnimationForBubble((neighbourData.bubble.transform.position - transform.position).normalized);
            }

            bubbleMesh.SetActive(false);
            explosionParticleEffect.gameObject.SetActive(true);

            List<Bubble> cachedBubblesToDeactivate = new List<Bubble>();
            foreach (var neighbourData in neighbourBubbles)
            {
                cachedBubblesToDeactivate.Add(neighbourData.bubble);
                LevelData.bubblesLevelDataDictionary.Remove(neighbourData.bubble.PositionID);
                ((BubbleColored)neighbourData.bubble).ActivateDeactivatedVFX();
                yield return new WaitForSeconds(0.1f);
            } 

            LevelData.bubblesLevelDataDictionary.Remove(PositionID);

            //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);

            yield return new WaitForSeconds(1f);

            cachedBubblesToDeactivate.ForEach(t => t.gameObject.SetActive(false));
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.MoveNextBubbleToCurrentBubble);
            gameObject.SetActive(false);
        }
    }
}
