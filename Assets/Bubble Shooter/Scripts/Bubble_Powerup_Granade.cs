using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class Bubble_Powerup_Granade : Bubble
    {
        [Header("Granade Powerup Data")]
        [SerializeField] private ParticleSystem initialGlow;
        [SerializeField] private int granadeDestructionLevel = 2;
        [SerializeField] private ParticleSystem explosionParticleEffect;

        protected override void OnLaunchBallSettleAtFinalPosition(Vector3 finalPoint, Bubble bubbleWeAreShootingTo)
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
                (neighbourData.bubble).PlayImpactMotionAnimationForBubble((neighbourData.bubble.transform.position - transform.position).normalized);
            }

            bubbleMesh.SetActive(false);
            explosionParticleEffect.gameObject.SetActive(true);

            List<Bubble> cachedBubblesToDeactivate = new List<Bubble>();
            foreach (var bubble in BubbleShooter_HelperFunctions.GetExploredBubblesOfCertainLevel(this, granadeDestructionLevel))
            {
                cachedBubblesToDeactivate.Add(bubble);
                LevelData.bubblesLevelDataDictionary.Remove(bubble.PositionID);
                bubble.ActivateDeactivatedVFX();
                yield return new WaitForSeconds(0.1f);
            }

            LevelData.bubblesLevelDataDictionary.Remove(PositionID);

            //Recalculating neighbour Data again for all board bubbles - because a new bubble got added to the board
            BubbleShooter_HelperFunctions.RecalculateAllBubblesNeighboursData(LevelData.bubblesLevelDataDictionary, LevelGenerator.bubbleGap);

            yield return new WaitForSeconds(0.5f);

            //Once bubble clears the similar colors, we need to seperate isolated bubbles from the level
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.OnBubbleCollisionClearDataComplete);

            yield return new WaitForSeconds(1.5f);

            cachedBubblesToDeactivate.ForEach(t => t.gameObject.SetActive(false));
            SNEventsController<InGameEvents>.TriggerEvent(InGameEvents.MoveNextBubbleToCurrentBubble);
            gameObject.SetActive(false);
        }

        public override void ActivateDeactivatedVFX()
        {
            BubbleMesh.SetActive(false);
        }

        protected override void InitOnExecutingLaunch()
        {
            initialGlow.gameObject.SetActive(false);
        }
    }
}
