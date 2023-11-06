using SNGames.CommonModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class LevelFailedDialog : BaseUIDialog
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GoalTarget goalTarget;

        private List<GoalTarget> spawnedGoalTargets = new List<GoalTarget>();

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();

            InitialGoalSetUp();
        }

        public override void OnCloseDialog()
        {
            base.OnCloseDialog();

            spawnedGoalTargets.ForEach(t => Destroy(t.gameObject));
        }

        public void InitialGoalSetUp()
        {
            spawnedGoalTargets = new List<GoalTarget>();
            foreach (var item in LevelData.currentLevelCurrentTargetStatus)
            {
                GoalTarget spawnedTarget = Instantiate(goalTarget);
                spawnedTarget.transform.SetParent(parentTransform);
                spawnedTarget.transform.gameObject.SetActive(true);

                bool shouldEnableWrongImage = false;
                if (item.Value > 0)
                    shouldEnableWrongImage = true;

                spawnedTarget.SetTarget(item.Value, inGameBubbleData.BubbleIdAndSprite[item.Key], !shouldEnableWrongImage, shouldEnableWrongImage);
            }
        }
    }
}
