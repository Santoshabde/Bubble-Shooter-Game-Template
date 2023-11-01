using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;
using TMPro;

namespace SNGames.BubbleShooter
{
    public class LevelTargetSSummaryDialog : BaseUIDialog
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private TextMeshProUGUI racoonsLivesToSave;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GoalTarget goalTarget;

        private LevelGenData currentLevelGenData = null;
        private List<GoalTarget> spawnedGoalTargets = new List<GoalTarget>();

        public override void OnOpenDialog()
        {
            base.OnOpenDialog();

            currentLevelGenData = LevelData.currentLevelGenData;
            InitialGoalSetUp(currentLevelGenData.targetBubbles);

            StartCoroutine(DisbleTheDialog());
        }

        public override void OnCloseDialog()
        {
            base.OnCloseDialog();

            spawnedGoalTargets.ForEach(t => Destroy(t.gameObject));
        }

        public void InitialGoalSetUp(List<TargetLevelBubble> targetBubbles)
        {
            spawnedGoalTargets = new List<GoalTarget>();
            foreach (var item in targetBubbles)
            {
                GoalTarget spawnedTarget = Instantiate(goalTarget);
                spawnedTarget.transform.SetParent(parentTransform);
                spawnedTarget.transform.gameObject.SetActive(true);
                spawnedTarget.SetTarget(item.targetNumber, inGameBubbleData.BubbleIdAndSprite[item.targetBubble]);
                spawnedGoalTargets.Add(spawnedTarget);
            }
        }

        private IEnumerator DisbleTheDialog()
        {
            yield return new WaitForSeconds(2f);
            OnCloseDialog();
        }
    }
}